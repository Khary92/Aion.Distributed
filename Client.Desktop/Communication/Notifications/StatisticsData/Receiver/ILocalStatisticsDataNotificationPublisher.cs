using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.StatisticsData.Records;

namespace Client.Desktop.Communication.Notifications.StatisticsData.Receiver;

public interface ILocalStatisticsDataNotificationPublisher
{
    event Func<ClientChangeProductivityNotification, Task>? ClientChangeProductivityNotificationReceived;
    event Func<ClientChangeTagSelectionNotification, Task>? ClientChangeTagSelectionNotificationReceived;
}