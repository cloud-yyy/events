using Application.Abstractions;

namespace Application.Events.DeleteEvent;

public record class DeleteEventCommand(Guid Id) : ICommand;
