using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.UseCase;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class TracingNotificationProcessor(IMessenger messenger) : INotificationHandler<TraceReportSentNotification>
{
    public Task Handle(TraceReportSentNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Unit.Task;
    }
}