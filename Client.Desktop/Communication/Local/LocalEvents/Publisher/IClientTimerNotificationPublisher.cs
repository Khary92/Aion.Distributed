using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Local.LocalEvents.Records;

namespace Client.Desktop.Communication.Local.LocalEvents.Publisher;

public interface IClientTimerNotificationPublisher
{
    event Func<ClientCreateSnapshotNotification, Task>? ClientCreateSnapshotNotificationReceived;
    event Func<ClientSaveDocumentationNotification, Task>? ClientSaveDocumentationNotificationReceived;
    
}