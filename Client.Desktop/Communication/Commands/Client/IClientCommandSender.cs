using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Client.Records;

namespace Client.Desktop.Communication.Commands.Client;

public interface IClientCommandSender
{
    Task<bool> Send(ClientCreateTrackingControlCommand command);
}