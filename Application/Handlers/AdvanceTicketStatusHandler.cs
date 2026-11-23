using MiniHelpDesk.Application.Commands;
using MiniHelpDesk.Domain;
using MiniHelpDesk.Infrastructure.Repositories;

namespace MiniHelpDesk.Application.Handlers;

public sealed class AdvanceTicketStatusHandler : ICommandHandler<AdvanceTicketStatusCommand>
{
    private readonly ITicketRepository _repository;
    public AdvanceTicketStatusHandler(ITicketRepository repository) => _repository = repository;

    public void Handle(AdvanceTicketStatusCommand command)
    {
        var ticket = _repository.Find(command.Id)
            ?? throw new KeyNotFoundException("Nie znaleziono zgłoszenia do zmiany statusu.");
        ticket.Status = ticket.Status switch
        {
            TicketOptions.New => TicketOptions.InProgress,
            TicketOptions.InProgress => TicketOptions.Closed,
            _ => TicketOptions.New
        };
    }
}
