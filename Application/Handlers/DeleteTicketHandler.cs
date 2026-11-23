using MiniHelpDesk.Application.Commands;
using MiniHelpDesk.Infrastructure.Repositories;

namespace MiniHelpDesk.Application.Handlers;

public sealed class DeleteTicketHandler : ICommandHandler<DeleteTicketCommand>
{
    private readonly ITicketRepository _repository;
    public DeleteTicketHandler(ITicketRepository repository) => _repository = repository;

    public void Handle(DeleteTicketCommand command)
    {
        if (!_repository.Remove(command.Id))
            throw new KeyNotFoundException("Nie znaleziono zgłoszenia do usunięcia.");
    }
}
