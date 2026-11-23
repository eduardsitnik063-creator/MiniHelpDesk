using Microsoft.Extensions.DependencyInjection;
using MiniHelpDesk.Application.Commands;
using MiniHelpDesk.Application.Handlers;
using MiniHelpDesk.Application.Queries;
using MiniHelpDesk.Infrastructure.Repositories;
using MiniHelpDesk.Presentation.ViewModels;

namespace MiniHelpDesk.App.ApplicationSetup;

public static class ServiceRegistration
{
    public static IServiceCollection AddMiniHelpDesk(this IServiceCollection services)
    {
        services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();
        services.AddSingleton<ICommandBus, CommandBus>();
        services.AddSingleton<ITicketQueries, TicketQueries>();
        services.AddSingleton<ICommandHandler<AddTicketCommand>, AddTicketHandler>();
        services.AddSingleton<ICommandHandler<EditTicketCommand>, EditTicketHandler>();
        services.AddSingleton<ICommandHandler<DeleteTicketCommand>, DeleteTicketHandler>();
        services.AddSingleton<ICommandHandler<AdvanceTicketStatusCommand>, AdvanceTicketStatusHandler>();
        services.AddSingleton<MainViewModel>();
        return services;
    }
}
