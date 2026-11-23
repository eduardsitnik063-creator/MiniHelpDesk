using System.Collections.ObjectModel;
using MiniHelpDesk.Domain;

namespace MiniHelpDesk.Application.Queries;

public sealed record TicketCounts(int All, int Active);

public interface ITicketQueries
{
    ReadOnlyObservableCollection<Ticket> GetAll();
    TicketCounts GetCounts();
}
