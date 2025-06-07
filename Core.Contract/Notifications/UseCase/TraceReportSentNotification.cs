using Contract.DTO;
using MediatR;

namespace Contract.Notifications.UseCase;

public record TraceReportSentNotification(TraceReportDto TraceReportDto) : INotification;