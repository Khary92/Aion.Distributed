using System.Threading.Tasks;
using Proto.Command.Tags;

namespace Client.Desktop.Communication.Commands.Tags;

public interface ITagCommandSender
{
    Task<bool> Send(CreateTagCommandProto command);
    Task<bool> Send(UpdateTagCommandProto command);
}