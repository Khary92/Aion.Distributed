using Proto.Command.Tags;

namespace Service.Admin.Web.Communication.Commands.Tags;

public interface ITagCommandSender
{
    Task<bool> Send(CreateTagCommandProto command);
    Task<bool> Send(UpdateTagCommandProto command);
}