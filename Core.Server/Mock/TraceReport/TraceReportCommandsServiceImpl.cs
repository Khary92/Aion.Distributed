using System;
using System.Threading.Tasks;
using Grpc.Core;
using Proto.Command.TraceReports;
using Proto.Notifications.UseCase;

namespace Service.Server.Mock;

public class TraceReportCommandServiceImpl(TraceReportNotificationServiceImpl notificationService)
    : TraceReportCommandService.TraceReportCommandServiceBase
{
    public override async Task<CommandResponse> SendTraceReport(SendTraceReportCommand request, ServerCallContext context)
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