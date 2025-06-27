using Microsoft.AspNetCore.Builder;
using Service.Monitoring.Communication;

namespace Service.Monitoring;

public static class EndpointServices
{
    public static void AddEndPoints(this WebApplication app)
    {
        AddCommandEndPoints(app);
    }

    private static void AddCommandEndPoints(WebApplication app)
    {
        app.MapGrpcService<TraceDataCommandReceiver>();
    }
}