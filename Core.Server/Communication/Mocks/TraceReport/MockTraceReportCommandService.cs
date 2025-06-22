using Grpc.Core;
using Proto.Command.TraceReports;
using Proto.Notifications.TraceReports;
using TraceReportNotificationService = Core.Server.Communication.Endpoints.TraceReport.TraceReportNotificationService;

namespace Core.Server.Communication.Mocks.TraceReport;

public class MockTraceReportCommandService(TraceReportNotificationService notificationService)
    : TraceReportCommandService.TraceReportCommandServiceBase
{
    public override async Task<CommandResponse> SendTraceReport(SendTraceReportCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[SendTraceReport] TraceReportDto: {request.TraceReportDto}");

        try
        {
            await notificationService.SendNotificationAsync(new TraceReportNotification
            {
                TraceReportSent = new TraceReportSentNotification
                {
                    TraceReportDto = request.TraceReportDto
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] SendTraceReport failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}