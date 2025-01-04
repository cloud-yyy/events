using Application.Dtos;
using Application.Events.CreateEvent;
using Ardalis.Result;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace Application.Tests.Events;

public class CreateEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly CreateEventCommand _command;
    private readonly Event _entity;

    public CreateEventCommandHandlerTests()
    {
        _eventRepositoryMock = new();
        _unitOfWorkMock = new();
        
        _entity = new Event { Name = "Name" };
        _command = new CreateEventCommand
            (_entity.Name, "", "", "", 1, _entity.Date);

        _mapperMock = new();
        _mapperMock
            .Setup(x => x.Map<EventDto>(It.IsAny<Event>()))
            .Returns((Event e) => Map(e));
    }

    [Fact]
    public async Task Handle_Should_ReturnInvalid_WhenEventWithSameNameExists()
    {
        // Arrange
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
    public async Task Handle_Should_ReturnSuccess_WhenEventNameIsUnique()
    {
        // Arrange
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
    public async Task Handle_Should_CallRepositoryAdd_WhenEventNameIsUnique()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);
    
        // Assert
        _eventRepositoryMock
            .Verify(x => x.Add(It.IsAny<Event>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWorkSaveChangesAsync_WhenEventNameIsUnique()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);
    
        // Assert
        _unitOfWorkMock
            .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private CreateEventCommandHandler CreateHandler()
    {
        return new CreateEventCommandHandler(
            _eventRepositoryMock.Object, 
            _unitOfWorkMock.Object, 
            _mapperMock.Object
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
            entity.CurrentParticipants,
            entity.MaxParticipants,
            entity.Date,
            new ImageDto(Guid.NewGuid(), "")
        );
    }
}
