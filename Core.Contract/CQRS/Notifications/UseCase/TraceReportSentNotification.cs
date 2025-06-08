using MediatR;

namespace Contract.CQRS.Notifications.UseCase;

public record TraceReportSentNotification(string TraceReportDto) : INotification;