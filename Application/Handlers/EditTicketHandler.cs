using MiniHelpDesk.Application.Commands;
using MiniHelpDesk.Application.Validation;
using MiniHelpDesk.Infrastructure.Repositories;

namespace MiniHelpDesk.Application.Handlers;

public sealed class EditTicketHandler : ICommandHandler<EditTicketCommand>
{
    private readonly ITicketRepository _repository;
    public EditTicketHandler(ITicketRepository repository) => _repository = repository;

    public void Handle(EditTicketCommand command)
    {
        var errors = TicketValidator.Validate(command.Title, command.Description, command.Priority);
        if (!errors.IsValid) throw new TicketValidationException(errors);
        var ticket = _repository.Find(command.Id)
            ?? throw new KeyNotFoundException("Nie znaleziono zgłoszenia do edycji.");
        ticket.Title = command.Title.Trim();
        ticket.Description = command.Description.Trim();
        ticket.Priority = command.Priority;
    }
}
