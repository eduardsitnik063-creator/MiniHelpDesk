using MiniHelpDesk.Presentation;

namespace MiniHelpDesk.Domain;

/*
klasa: Ticket

opis:
Reprezentuje pojedyncze zgłoszenie techniczne. Informuje o zmianach pól przez INotifyPropertyChanged.

pola / właściwości:
Id, Guid, identyfikator zgłoszenia
Title, string, nazwa zgłoszenia
Description, string, opis problemu
Priority, string, priorytet zgłoszenia
Status, string, stan realizacji zgłoszenia
*/
public sealed class Ticket : ObservableBase
{
    private string _title;
    private string _description;
    private string _priority;
    private string _status = TicketOptions.New;

    public Ticket(Guid id, string title, string description, string priority)
    {
        Id = id;
        _title = title;
        _description = description;
        _priority = priority;
    }

    public Guid Id { get; }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string Description { get => _description; set => SetProperty(ref _description, value); }
    public string Priority { get => _priority; set => SetProperty(ref _priority, value); }
    public string Status { get => _status; set => SetProperty(ref _status, value); }
}
