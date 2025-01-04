using Application.Dtos;
using Application.Events.GetEventById;
using Ardalis.Result;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace Application.Tests.Events;

public class GetEventByIdQueryHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly Event _entity;
    private readonly EventDto _dto;

    public GetEventByIdQueryHandlerTests()
    {
        _eventRepositoryMock = new();
        _mapperMock = new();

        _entity = new(){ Id = Guid.NewGuid()};
        _dto = new(
            _entity.Id, "", "", "", "", 0, 10, DateOnly.FromDateTime(DateTime.UtcNow), new ImageDto()
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenEventNotExistsAsync()
    {
        // Arrange
        var query = new GetEventByIdQuery(Guid.NewGuid());
    
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);
        
        // Act
        var result = await CreateHandler().Handle(query, CancellationToken.None);

        // Assert
        result.IsNotFound().Should().BeTrue();
        result.Errors.First().Should().Be($"Event with id {query.Id} not found");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenEventExists()
    {
        // Arrange
        var query = new GetEventByIdQuery(_entity.Id);
    
        _eventRepositoryMock
            .Setup(x => x.GetByIdAsync(_entity.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_entity);
        
        _mapperMock
            .Setup(x => x.Map<EventDto>(_entity))
            .Returns(_dto);
        
        // Act
        var result = await CreateHandler().Handle(query, CancellationToken.None);
    
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(_entity.Id);
    }

    private GetEventByIdQueryHandler CreateHandler()
    {
        return new GetEventByIdQueryHandler(
            _eventRepositoryMock.Object, 
            _mapperMock.Object
        );
    }
}
