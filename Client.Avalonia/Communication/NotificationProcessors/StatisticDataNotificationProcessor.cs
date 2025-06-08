using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class StatisticDataNotificationProcessor(IMessenger messenger) :
    INotificationHandler<CreateStatisticsDataCommand>,
    INotificationHandler<ChangeProductivityCommand>,
    INotificationHandler<ChangeTagSelectionCommand>
{
    public Task Handle(CreateStatisticsDataCommand notification, CancellationToken cancellationToken)
    {
        messenger.Send(new NewStatisticDataMessage());
        return Task.CompletedTask;
    }

    public Task Handle(ChangeProductivityCommand notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }

    public Task Handle(ChangeTagSelectionCommand notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }
}