using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Local.LocalEvents.Records;

namespace Client.Desktop.Communication.Local.LocalEvents.Publisher;

public interface IClientWorkDaySelectionChangedNotificationPublisher
{
    event Func<ClientWorkDaySelectionChangedNotification, Task>? ClientWorkDaySelectionChangedNotificationReceived;
}