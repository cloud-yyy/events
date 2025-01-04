using Application.Dtos;
using Application.Events.GetAllEvents;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using Persistence;

namespace Application.Tests.Events;

public class GetAllEventsQueryHandlerTests
{
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly IEnumerable<Event> _entities;
    private readonly IEnumerable<EventDto> _dtos;

    public GetAllEventsQueryHandlerTests()
    {
        _eventRepositoryMock = new();
        
        _mapperMock = new();
        _mapperMock
            .Setup(m => m.Map<EventDto>(It.IsAny<Event>()))
            .Returns((Event e) => Map(e));

        _entities = 
        [
            new(){ 
                Id = Guid.NewGuid(), 
                Date = new DateOnly(2000, 01, 01), 
                Place = "Place 1", 
                Category = "Category 1",
                Image = new Image()
            },
            new(){ 
                Id = Guid.NewGuid(), 
                Date = new DateOnly(2000, 01, 02), 
                Place = "Place 2", 
                Category = "Category 2",
                Image = new Image()
            },
            new(){ 
                Id = Guid.NewGuid(), 
                Date = new DateOnly(2000, 01, 03), 
                Place = "Place 3", 
                Category = "Category 3",
                Image = new Image()
            }
        ];

        _dtos = _entities.Select(Map);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllEvents_WhenNoFiltersApplied()
    {
        // Arrange
        var filter = new EventFilter();
        var query = new GetAllEventsQuery(1, 3);
    
        _eventRepositoryMock
            .Setup(x => x.GetAllAsync(query.PageNumber, query.PageSize, filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PagedList<Event>.Create(_entities.AsQueryable(), query.PageNumber, query.PageSize));
        
        // Act
        var result = await CreateHandler().Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo(_dtos);
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrespondingEvents_WhenDateFilterApplied()
    {
        // Arrange
        var entity = _entities.First();
        var expected = _dtos.First();
        var filter = new EventFilter(Date: entity.Date);
        var query = new GetAllEventsQuery(1, 3, entity.Date);
    
        _eventRepositoryMock
            .Setup(x => x.GetAllAsync(query.PageNumber, query.PageSize, filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PagedList<Event>.Create(
                _entities.Where(e => e.Date == filter.Date).AsQueryable(), 
                query.PageNumber, 
                query.PageSize)
            );
        
        // Act
        var result = await CreateHandler().Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo([expected]);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllEvents_WhenPlaceFilterApplied()
    {
        // Arrange
        var entity = _entities.First();
        var expected = _dtos.First();
        var filter = new EventFilter(Place: entity.Place);
        var query = new GetAllEventsQuery(1, 3, Place: entity.Place);
    
        _eventRepositoryMock
            .Setup(x => x.GetAllAsync(query.PageNumber, query.PageSize, filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PagedList<Event>.Create(
                _entities.Where(e => e.Place == filter.Place).AsQueryable(), 
                query.PageNumber, 
                query.PageSize)
            );

        // Act
        var result = await CreateHandler().Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo([expected]);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllEvents_WhenCategoryFilterApplied()
    {
        // Arrange
        var entity = _entities.First();
        var expected = _dtos.First();
        var filter = new EventFilter(Category: entity.Category);
        var query = new GetAllEventsQuery(1, 3, Category: entity.Category);
    
        _eventRepositoryMock
            .Setup(x => x.GetAllAsync(query.PageNumber, query.PageSize, filter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PagedList<Event>.Create(
                _entities.Where(e => e.Category == filter.Category).AsQueryable(), 
                query.PageNumber, 
                query.PageSize)
            );

        // Act
        var result = await CreateHandler().Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEquivalentTo([expected]);
    }

    private GetAllEventsQueryHandler CreateHandler()
    {
        return new GetAllEventsQueryHandler(
            _eventRepositoryMock.Object, 
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
            new ImageDto(entity.Image!.Id, entity.Image.ObjectKey)
        );
    }
}
