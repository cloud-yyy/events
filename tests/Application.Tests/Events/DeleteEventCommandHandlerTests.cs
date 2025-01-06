using Application.Abstractions;
using Application.ErrorResults;
using Application.Events.DeleteEvent;
using Ardalis.Result;
using Domain;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Application.Tests.Events;

public class DeleteEventCommandHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<LinkGenerator> _linkGeneratorMock;
    private readonly Mock<IS3Client> _s3ClientMock;
    private readonly LinkFactory _linkFactory;

    private readonly DeleteEventCommand _command;
    private readonly Event _entity;

    public DeleteEventCommandHandlerTests()
    {
        _eventRepositoryMock = new();
        _unitOfWorkMock = new();
        _emailSenderMock = new();
        _s3ClientMock = new();
        _imageRepositoryMock = new();
        
        _httpContextAccessorMock = new();
        _linkGeneratorMock = new();
        _linkFactory = new(_httpContextAccessorMock.Object, _linkGeneratorMock.Object);
        
        _command = new DeleteEventCommand(Guid.NewGuid());

        _entity = new Event 
        {
            Id = _command.Id, 
            Participants = [new() , new(), new()],
            Image = new Image()
        };

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
        result.Errors.Should()
            .BeEquivalentTo(EventResults.NotFound.ById(_command.Id).Errors);
    }

    [Fact]
    public async Task Handle_Should_ReturnNoContent_WhenCommandIsValid()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);

        // Assert
        result.IsNoContent().Should().BeTrue();
    }
    
    [Fact]
    public async Task Handle_Should_CallRepositoryDelete_WhenCommandIsValid()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);

        // Assert
        _eventRepositoryMock
            .Verify(x => x.Delete(_entity), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_CallContextSaveChangesAsync_WhenCommandIsValid()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

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

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);

        // Assert
        _emailSenderMock
            .Verify(x => x.SendManyAsync(
                It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>()), 
                Times.Once()
            );
    }

    [Fact]
    public async Task Handle_Should_CallImageRepositoryDelete_WhenImageIsNotNull()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);

        // Assert
        _imageRepositoryMock
            .Verify(x => x.Delete(_entity.Image!), Times.Once());
    }

    [Fact]
    public async Task Handle_Should_CallS3ClientDeleteAsync_WhenImageIsNotNull()
    {
        // Arrange
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);

        // Act
        var result = await CreateHandler().Handle(_command, CancellationToken.None);

        // Assert
        _s3ClientMock
            .Verify(x => x.DeleteFileAsync(
                _entity.Image!.BucketName, _entity.Image.ObjectKey, It.IsAny<CancellationToken>()), 
                Times.Once()
            );
    }

    private DeleteEventCommandHandler CreateHandler()
    {
        return new DeleteEventCommandHandler(
            _eventRepositoryMock.Object, 
            _imageRepositoryMock.Object, 
            _unitOfWorkMock.Object, 
            _emailSenderMock.Object,
            _linkFactory,
            _s3ClientMock.Object
        );
    }
}
