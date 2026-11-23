using System.Collections.ObjectModel;
using MiniHelpDesk.Domain;

namespace MiniHelpDesk.Infrastructure.Repositories;

public interface ITicketRepository
{
    ReadOnlyObservableCollection<Ticket> Tickets { get; }
    event EventHandler? Changed;
    Ticket? Find(Guid id);
    void Add(Ticket ticket);
    bool Remove(Guid id);
}
