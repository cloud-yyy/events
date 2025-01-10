using System.Runtime.CompilerServices;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace Persistence.Tests.Repositories
{
    public class EventRepositoryTests
    {
        private readonly Event _entity = new()
        {
            Id = Guid.NewGuid(),
            Image = new(),
            Category = new(),
            Participants = { new User() }
        };

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNull_WhenEventDoesNotExist()
        {
            // Arrange
            using var context = new ApplicationDbContext(CreateDatabase());

            var repository = new EventRepository(context);

            // Act
            var entity = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            entity.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnEvent_WhenEventExists()
        {
            // Arrange
            using var context = new ApplicationDbContext(CreateDatabase());

            context.Events.Add(_entity);
            await context.SaveChangesAsync();

            var repository = new EventRepository(context);

            // Act
            var entity = await repository.GetByIdAsync(_entity.Id);

            // Assert
            entity.Should().NotBeNull();
            entity!.Id.Should().Be(_entity.Id);
            entity!.Category.Should().NotBeNull();
            entity!.Category!.Id.Should().Be(_entity.Category!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Should_IncludeImage_WhenEventExistsAndHasImage()
        {
            // Arrange
            using var context = new ApplicationDbContext(CreateDatabase());
            
            context.Events.Add(_entity);
            await context.SaveChangesAsync();
            
            var repository = new EventRepository(context);
            // Act
            var entity = await repository.GetByIdAsync(_entity.Id);
        
            // Assert
            entity.Should().NotBeNull();
            entity!.Image.Should().NotBeNull();
            entity!.Image!.Id.Should().Be(_entity.Image!.Id);
        }

        [Fact]
        public void Add_Should_ThrowException_WhenEventIsNull()
        {
            // Arrange
            using var context = new ApplicationDbContext(CreateDatabase());

            var repository = new EventRepository(context);

            // Act
            Action act = () => repository.Add(null!);

            // Assert
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public async Task Add_Should_AddEvent_WhenEventIsValid()
        {
            // Arrange
            using var context = new ApplicationDbContext(CreateDatabase());

            var repository = new EventRepository(context);
            // Act
            repository.Add(_entity);
            context.SaveChanges();

            // Assert
            var count = await context.Events.CountAsync();
            count.Should().Be(1);
            var added = await context.Events.SingleAsync();
            added.Id.Should().Be(_entity.Id);
        }

        private static DbContextOptions<ApplicationDbContext> CreateDatabase(
            [CallerMemberName] string name = "")
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
        }
    }
}
