using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Local.LocalEvents.Records;

namespace Client.Desktop.Test;

public class TestTimerSettingsPublisher : IClientTimerNotificationPublisher
{
    public event Func<ClientCreateSnapshotNotification, Task>? ClientCreateSnapshotNotificationReceived;
    public event Func<ClientSaveDocumentationNotification, Task>? ClientSaveDocumentationNotificationReceived;

    public async Task Publish(ClientCreateSnapshotNotification notification)
    {
        await ClientCreateSnapshotNotificationReceived!.Invoke(notification);
    }

    public async Task Publish(ClientSaveDocumentationNotification notification)
    {
        await ClientSaveDocumentationNotificationReceived!.Invoke(notification);
    }
}