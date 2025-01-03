using Application.Dtos;
using Application.Events.GetEventByName;
using Ardalis.Result;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace Application.Tests.Events;

public class GetEventByNameQueryHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly Event _entity;
    private readonly EventDto _dto;

    public GetEventByNameQueryHandlerTests()
    {
        _eventRepositoryMock = new();
        _mapperMock = new();

        _entity = new(){ Id = Guid.NewGuid(), Name = "Name"};
        _dto = new(
            _entity.Id, _entity.Name, "", "", "", 0, DateOnly.FromDateTime(DateTime.UtcNow), new ImageDto()
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenEventNotExistsAsync()
    {
        // Arrange
        var query = new GetEventByNameQuery("Nonexisted name");
    
        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(query.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        // Act
        var result = await CreateHandler().Handle(query, CancellationToken.None);

        // Assert
        result.IsNotFound().Should().BeTrue();
        result.Errors.First().Should().Be($"Event with name {query.Name} not found");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenEventExists()
    {
        // Arrange
        var query = new GetEventByNameQuery(_entity.Name);
    
        _eventRepositoryMock
            .Setup(x => x.GetByNameAsync(_entity.Name, It.IsAny<CancellationToken>()))
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

    private GetEventByNameQueryHandler CreateHandler()
    {
        return new GetEventByNameQueryHandler(
            _eventRepositoryMock.Object, 
            _mapperMock.Object
        );
    }
}
