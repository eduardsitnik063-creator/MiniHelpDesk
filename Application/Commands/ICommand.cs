namespace MiniHelpDesk.Application.Commands;

public interface ICommand { }

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    void Handle(TCommand command);
}

public interface ICommandBus
{
    void Send<TCommand>(TCommand command) where TCommand : ICommand;
}
