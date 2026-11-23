using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace MiniHelpDesk.Application.Commands;

/*
klasa: CommandBus

opis:
Przekazuje komendy CQRS do odpowiednich handlerów z kontenera DI.

pola / właściwości:
_services, IServiceProvider, kontener zależności aplikacji
*/
public sealed class CommandBus : ICommandBus
{
    private readonly IServiceProvider _services;
    public CommandBus(IServiceProvider services) => _services = services;

    public void Send<TCommand>(TCommand command) where TCommand : ICommand
        => _services.GetRequiredService<ICommandHandler<TCommand>>().Handle(command);
}
