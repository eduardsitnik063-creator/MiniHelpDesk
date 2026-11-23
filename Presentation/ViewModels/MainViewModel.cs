using System.Collections.ObjectModel;
using MiniHelpDesk.Application.Commands;
using MiniHelpDesk.Application.Queries;
using MiniHelpDesk.Application.Validation;
using MiniHelpDesk.Domain;
using MiniHelpDesk.Infrastructure.Repositories;

namespace MiniHelpDesk.Presentation.ViewModels;

/*
klasa: MainViewModel

opis:
Obsługuje formularz oraz obserwowalny stan widoku. Zleca operacje przez Command Bus.

pola / właściwości:
Tickets, ReadOnlyObservableCollection<Ticket>, zgłoszenia wyświetlane w interfejsie
TitleInput, string, nazwa w formularzu
DescriptionInput, string, opis w formularzu
SelectedPriority, string?, priorytet wybrany w formularzu
TotalCount, int, liczba wszystkich zgłoszeń
ActiveCount, int, liczba niezakończonych zgłoszeń
*/
public sealed class MainViewModel : ObservableBase
{
    private readonly ICommandBus _commands;
    private readonly ITicketQueries _queries;
    private string _titleInput = "";
    private string _descriptionInput = "";
    private string? _selectedPriority;
    private string _titleError = "";
    private string _descriptionError = "";
    private string _priorityError = "";
    private int _totalCount;
    private int _activeCount;
    private Ticket? _editingTicket;
    private bool _showAllErrors;

    public MainViewModel(ICommandBus commands, ITicketQueries queries, ITicketRepository repository)
    {
        _commands = commands;
        _queries = queries;
        Tickets = queries.GetAll();
        repository.Changed += OnRepositoryChanged;
        UpdateCounts();
    }

    public ReadOnlyObservableCollection<Ticket> Tickets { get; }
    public IReadOnlyList<string> Priorities => TicketOptions.Priorities;

    public string TitleInput
    {
        get => _titleInput;
        set { if (SetProperty(ref _titleInput, value ?? "")) UpdateValidation(); }
    }

    public string DescriptionInput
    {
        get => _descriptionInput;
        set { if (SetProperty(ref _descriptionInput, value ?? "")) UpdateValidation(); }
    }

    public string? SelectedPriority
    {
        get => _selectedPriority;
        set { if (SetProperty(ref _selectedPriority, value)) UpdateValidation(); }
    }

    public string TitleError { get => _titleError; private set => SetProperty(ref _titleError, value); }
    public string DescriptionError { get => _descriptionError; private set => SetProperty(ref _descriptionError, value); }
    public string PriorityError { get => _priorityError; private set => SetProperty(ref _priorityError, value); }
    public int TotalCount { get => _totalCount; private set => SetProperty(ref _totalCount, value); }
    public int ActiveCount { get => _activeCount; private set => SetProperty(ref _activeCount, value); }
    public string SaveButtonText => _editingTicket is null ? "Dodaj" : "Zapisz zmiany";

    /*
    funkcja: TrySave

    opis:
    Sprawdza formularz i wysyła komendę dodania lub edycji zgłoszenia.

    parametry:
    brak

    zwraca:
    bool, true po poprawnym zapisie lub false przy błędnych danych
    */
    public bool TrySave()
    {
        _showAllErrors = true;
        UpdateValidation();
        if (!TicketValidator.Validate(TitleInput, DescriptionInput, SelectedPriority).IsValid) return false;

        try
        {
            if (_editingTicket is null)
                _commands.Send(new AddTicketCommand(TitleInput, DescriptionInput, SelectedPriority!));
            else
                _commands.Send(new EditTicketCommand(_editingTicket.Id, TitleInput, DescriptionInput, SelectedPriority!));
        }
        catch (TicketValidationException ex)
        {
            TitleError = ex.Errors.TitleError;
            DescriptionError = ex.Errors.DescriptionError;
            PriorityError = ex.Errors.PriorityError;
            return false;
        }
        ResetForm();
        return true;
    }

    public void StartEditing(Ticket? ticket)
    {
        if (ticket is null || !Tickets.Contains(ticket)) return;
        _editingTicket = ticket;
        _showAllErrors = false;
        TitleInput = ticket.Title;
        DescriptionInput = ticket.Description;
        SelectedPriority = ticket.Priority;
        UpdateValidation();
        OnPropertyChanged(nameof(SaveButtonText));
    }

    public void Delete(Ticket? ticket)
    {
        if (ticket is null || !Tickets.Contains(ticket)) return;
        if (ReferenceEquals(_editingTicket, ticket)) ResetForm();
        _commands.Send(new DeleteTicketCommand(ticket.Id));
    }

    public void AdvanceStatus(Ticket? ticket)
    {
        if (ticket is null || !Tickets.Contains(ticket)) return;
        _commands.Send(new AdvanceTicketStatusCommand(ticket.Id));
    }

    public void ResetForm()
    {
        _editingTicket = null;
        _showAllErrors = false;
        TitleInput = "";
        DescriptionInput = "";
        SelectedPriority = null;
        UpdateValidation();
        OnPropertyChanged(nameof(SaveButtonText));
    }

    private void UpdateValidation()
    {
        var result = TicketValidator.Validate(TitleInput, DescriptionInput, SelectedPriority);
        TitleError = _showAllErrors || TitleInput.Length > 0 ? result.TitleError : "";
        DescriptionError = _showAllErrors || DescriptionInput.Length > 0 ? result.DescriptionError : "";
        PriorityError = _showAllErrors ? result.PriorityError : "";
    }

    private void OnRepositoryChanged(object? sender, EventArgs e) => UpdateCounts();

    private void UpdateCounts()
    {
        var counts = _queries.GetCounts();
        TotalCount = counts.All;
        ActiveCount = counts.Active;
    }
}
