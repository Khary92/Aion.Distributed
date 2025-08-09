using Microsoft.Extensions.DependencyInjection;
using Service.Monitoring.Communication;
using Service.Monitoring.Tracers;
using Service.Monitoring.Verifiers.Common;
using Service.Monitoring.Verifiers.Common.Factories;
using Service.Monitoring.Verifiers.Steps;

namespace Service.Monitoring;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddSinks(services);
        AddVerifiers(services);

        services.AddSingleton<IReportSender>(sp => new ReportSender("http://admin-web:8081"));
    }

    private static void AddSinks(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink, TraceSink>();
    }

    private static void AddVerifiers(this IServiceCollection services)
    {
        services.AddSingleton<IVerifierFactory, VerifierFactory>();

        services.AddSingleton<IVerificationProvider, NoteTypeVerificationProvider>();
        services.AddSingleton<IVerificationProvider, NoteVerificationProvider>();
        services.AddSingleton<IVerificationProvider, SprintVerificationProvider>();
        services.AddSingleton<IVerificationProvider, StatisticsDataVerificationProvider>();
        services.AddSingleton<IVerificationProvider, TagVerificationProvider>();
        services.AddSingleton<IVerificationProvider, TicketVerificationProvider>();
        services.AddSingleton<IVerificationProvider, TimerSettingsVerificationProvider>();
        services.AddSingleton<IVerificationProvider, WorkDayVerificationProvider>();
    }
}