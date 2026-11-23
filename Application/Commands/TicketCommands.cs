namespace MiniHelpDesk.Application.Commands;

public class AddTicketCommand : ICommand
{
    public string Title { get; }
    public string Description { get; }
    public string Priority { get; }

    public AddTicketCommand(string title, string description, string priority)
    {
        Title = title;
        Description = description;
        Priority = priority;
    }
}

public class EditTicketCommand : ICommand
{
    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }
    public string Priority { get; }

    public EditTicketCommand(Guid id, string title, string description, string priority)
    {
        Id = id;
        Title = title;
        Description = description;
        Priority = priority;
    }
}

public class DeleteTicketCommand : ICommand
{
    public Guid Id { get; }

    public DeleteTicketCommand(Guid id)
    {
        Id = id;
    }
}

public class AdvanceTicketStatusCommand : ICommand
{
    public Guid Id { get; }

    public AdvanceTicketStatusCommand(Guid id)
    {
        Id = id;
    }
}
