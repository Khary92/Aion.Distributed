using Client.Desktop.Communication.Commands.Tickets;
using Microsoft.Extensions.DependencyInjection;
using Service.Proto.Shared.Commands.Ticket;
using Service.Proto.Shared.Requests.Tickets;

namespace Service.Proto.Shared;

public static class SharedDataServices
{
    public static void AddSharedDataServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketCommandSender, TicketCommandSender>();
        services.AddScoped<ITicketRequestSender, TicketRequestSender>();
    }
}