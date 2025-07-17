using Microsoft.Extensions.DependencyInjection;
using Service.Admin.Tracing.Tracing.Ticket;
using Service.Admin.Tracing.Tracing.Ticket.UseCase;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddACommonTracingServices(services);
        AddTicketTracingServices(services);
    }

    private static void AddACommonTracingServices(this IServiceCollection services)
    {
        services.AddScoped<ITracingDataCommandSender>(sp => new TracingDataCommandSender("http://monitoring-service:8080"));
        services.AddSingleton<ITraceCollector, TraceCollector>();
    }

    private static void AddTicketTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateTicketTraceCollector, CreateTicketTraceCollector>();
        services.AddSingleton<IAddTicketToCurrentSprintTraceCollector, AddTicketToCurrentSprintTraceCollector>();
        services.AddSingleton<IUpdateTicketTraceCollector, UpdateTicketTraceCollector>();

        services.AddSingleton<ITicketUseCaseSelector, TicketUseCaseSelector>();
    }
}