using System.Threading.Tasks;

namespace Client.Desktop.Communication.Notifications;

public interface INotificationReceiverStarter
{
    Task Start();
}