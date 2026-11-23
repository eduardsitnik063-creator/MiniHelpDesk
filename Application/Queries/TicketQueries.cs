using System.Collections.ObjectModel;
using MiniHelpDesk.Domain;
using MiniHelpDesk.Infrastructure.Repositories;

namespace MiniHelpDesk.Application.Queries;

public sealed class TicketQueries : ITicketQueries
{
    private readonly ITicketRepository _repository;
    public TicketQueries(ITicketRepository repository) => _repository = repository;

    public ReadOnlyObservableCollection<Ticket> GetAll() => _repository.Tickets;

    public TicketCounts GetCounts()
        => new(_repository.Tickets.Count,
            _repository.Tickets.Count(ticket => ticket.Status != TicketOptions.Closed));
}
