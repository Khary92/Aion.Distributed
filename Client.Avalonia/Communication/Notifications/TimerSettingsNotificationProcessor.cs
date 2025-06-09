using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.TimerSettings;
using Contract.CQRS.Notifications.UseCase;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class TimerSettingsNotificationProcessor(IMessenger messenger) :
    INotificationHandler<TimerSettingsCreatedNotification>,
    INotificationHandler<DocuTimerSaveIntervalChangedNotification>,
    INotificationHandler<SnapshotSaveIntervalChangedNotification>,
    INotificationHandler<CreateSnapshotNotification>,
    INotificationHandler<SaveDocumentationNotification>
{
    public Task Handle(CreateSnapshotNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }

    public Task Handle(DocuTimerSaveIntervalChangedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }

    public Task Handle(SaveDocumentationNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }

    public Task Handle(SnapshotSaveIntervalChangedNotification notification, CancellationToken cancellationToken)
    {
        messenger.Send(notification);
        return Task.CompletedTask;
    }

    public Task Handle(TimerSettingsCreatedNotification notification, CancellationToken cancellationToken)
    {
        var timerSettings = new TimerSettingsDto(notification.TimerSettingsId, notification.DocumentationSaveInterval,
            notification.SnapshotSaveInterval);

        messenger.Send(new NewTimerSettingsMessage(timerSettings));

        return Task.CompletedTask;
    }
}