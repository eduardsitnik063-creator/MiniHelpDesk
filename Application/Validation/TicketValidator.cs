using MiniHelpDesk.Domain;

namespace MiniHelpDesk.Application.Validation;

public sealed record TicketValidationResult(
    string TitleError, string DescriptionError, string PriorityError)
{
    public bool IsValid => TitleError.Length == 0 && DescriptionError.Length == 0 && PriorityError.Length == 0;
}

/*
klasa: TicketValidator

opis:
Sprawdza poprawność danych zgłoszenia niezależnie od interfejsu użytkownika.

pola / właściwości:
brak
*/
public static class TicketValidator
{
    /*
    funkcja: Validate

    opis:
    Sprawdza długość nazwy i opisu oraz dopuszczalną wartość priorytetu.

    parametry:
    title, string?, nazwa zgłoszenia
    description, string?, opis zgłoszenia
    priority, string?, wybrany priorytet

    zwraca:
    TicketValidationResult, informacje o błędach i poprawności danych
    */
    public static TicketValidationResult Validate(string? title, string? description, string? priority)
    {
        var titleError = (title?.Trim().Length ?? 0) < 3 ? "Nazwa: minimum 3 znaki." : "";
        var descriptionError = (description?.Trim().Length ?? 0) < 10 ? "Opis: minimum 10 znaków." : "";
        var priorityError = !TicketOptions.Priorities.Contains(priority ?? "") ? "Wybierz priorytet." : "";
        return new(titleError, descriptionError, priorityError);
    }
}

public sealed class TicketValidationException : Exception
{
    public TicketValidationException(TicketValidationResult errors)
        : base("Niepoprawne dane zgłoszenia.") => Errors = errors;

    public TicketValidationResult Errors { get; }
}
