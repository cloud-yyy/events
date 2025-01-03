using Application.Abstractions;
using Application.Dtos;
using Application.Events.UpdateEvent;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Application.Tests.Events;

public class UpdateEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly LinkFactory _linkFactory;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<LinkGenerator> _linkGeneratorMock;

    private readonly UpdateEventCommand _command;
    private readonly Event _entity;

    public UpdateEventCommandHandlerTests()
    {
        _eventRepositoryMock = new();
        _unitOfWorkMock = new();
        _emailSenderMock = new();
        
        _httpContextAccessorMock = new();
        _linkGeneratorMock = new();
        _linkFactory = new(_httpContextAccessorMock.Object, _linkGeneratorMock.Object);
        
        _command = new UpdateEventCommand(
            Guid.NewGuid(), 
            "Name", 
            "Description", 
            "Place", 
            "Category", 
            1, 
            DateOnly.FromDateTime(DateTime.UtcNow)
        );

        _entity = new Event { Id = _command.Id, Participants = [new(), new(), new()] };

        _mapperMock = new();
        _mapperMock
            .Setup(x => x.Map<EventDto>(It.IsAny<Event>()))
            .Returns((Event e) => Map(e));

        _httpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext());
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenEventNotExists()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);

        // Assert
        result.IsNotFound().Should().BeTrue();
        result.Errors.First().Should().Be($"Event with id {_command.Id} not found");
    }

    [Fact]
    public async Task Handle_Should_ReturnInvalid_WhenEventWithSameAsCommandNameExists()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);

        // Assert
        result.IsInvalid().Should().BeTrue();
        result.ValidationErrors.First().ErrorMessage
            .Should().Be($"Event with name {_command.Name} already exists");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);
    
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(result.Value.Id);
    }

    [Fact]
    public async Task Handle_Should_CallRepositoryUpdate_WhenCommandIsValid()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);
        
        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);
    
        // Assert
        _eventRepositoryMock
            .Verify(x => x.Update(_entity), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWorkSaveChangesAsync_WhenCommandIsValid()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);
        
        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);
    
        // Assert
        _unitOfWorkMock
            .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_CallEmailSenderSendManyAsync_WhenCommandIsValid()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);
        
        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);
    
        // Assert
        _emailSenderMock
            .Verify(x => x.SendManyAsync(
                It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once
            );
    }

    private UpdateEventCommandHandler CreateHandler()
    {
        return new UpdateEventCommandHandler(
            _eventRepositoryMock.Object, 
            _unitOfWorkMock.Object, 
            _mapperMock.Object,
            _emailSenderMock.Object,
            _linkFactory
        );
    }

    private static EventDto Map(Event entity)
    {
        return new EventDto(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Place,
            entity.Category,
            entity.MaxParticipants,
            entity.Date,
            new ImageDto(Guid.NewGuid(), "")
        );
    }
}
