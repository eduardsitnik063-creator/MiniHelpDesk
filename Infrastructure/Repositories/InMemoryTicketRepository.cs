using System.Collections.ObjectModel;
using System.ComponentModel;
using MiniHelpDesk.Domain;

namespace MiniHelpDesk.Infrastructure.Repositories;

/*
klasa: InMemoryTicketRepository

opis:
Przechowuje zgłoszenia w pamięci i informuje obserwatorów o zmianach.

pola / właściwości:
_items, ObservableCollection<Ticket>, modyfikowalna kolekcja zgłoszeń
Tickets, ReadOnlyObservableCollection<Ticket>, kolekcja udostępniana do odczytu
Changed, EventHandler?, zdarzenie informujące o zmianie danych
*/
public sealed class InMemoryTicketRepository : ITicketRepository
{
    private readonly ObservableCollection<Ticket> _items = new();
    public ReadOnlyObservableCollection<Ticket> Tickets { get; }
    public event EventHandler? Changed;

    public InMemoryTicketRepository()
        => Tickets = new ReadOnlyObservableCollection<Ticket>(_items);

    public Ticket? Find(Guid id) => _items.FirstOrDefault(ticket => ticket.Id == id);

    /*
    funkcja: Add

    opis:
    Dodaje zgłoszenie i subskrybuje jego zmiany, a następnie informuje obserwatorów.

    parametry:
    ticket, Ticket, zgłoszenie do zapisania

    zwraca:
    void, brak zwracanej wartości
    */
    public void Add(Ticket ticket)
    {
        if (Find(ticket.Id) is not null) throw new InvalidOperationException("Zgłoszenie już istnieje.");
        ticket.PropertyChanged += OnTicketPropertyChanged;
        _items.Add(ticket);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public bool Remove(Guid id)
    {
        var ticket = Find(id);
        if (ticket is null) return false;
        ticket.PropertyChanged -= OnTicketPropertyChanged;
        _items.Remove(ticket);
        Changed?.Invoke(this, EventArgs.Empty);
        return true;
    }

    private void OnTicketPropertyChanged(object? sender, PropertyChangedEventArgs e)
        => Changed?.Invoke(this, EventArgs.Empty);
}
