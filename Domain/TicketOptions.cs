namespace MiniHelpDesk.Domain;

public static class TicketOptions
{
    public const string New = "Nowe";
    public const string InProgress = "W trakcie";
    public const string Closed = "Zamknięte";

    public static IReadOnlyList<string> Priorities { get; } =
        Array.AsReadOnly(new[] { "Niski", "Średni", "Wysoki" });
}
