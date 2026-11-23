using MiniHelpDesk.Application.Commands;
using MiniHelpDesk.Application.Validation;
using MiniHelpDesk.Domain;
using MiniHelpDesk.Infrastructure.Repositories;

namespace MiniHelpDesk.Application.Handlers;

public sealed class AddTicketHandler : ICommandHandler<AddTicketCommand>
{
    private readonly ITicketRepository _repository;
    public AddTicketHandler(ITicketRepository repository) => _repository = repository;

    public void Handle(AddTicketCommand command)
    {
        var errors = TicketValidator.Validate(command.Title, command.Description, command.Priority);
        if (!errors.IsValid) throw new TicketValidationException(errors);
        _repository.Add(new Ticket(Guid.NewGuid(), command.Title.Trim(),
            command.Description.Trim(), command.Priority));
    }
}
