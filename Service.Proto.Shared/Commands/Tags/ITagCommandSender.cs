using Proto.Command.Tags;

namespace Service.Proto.Shared.Commands.Tags;

public interface ITagCommandSender
{
    Task<bool> Send(CreateTagCommandProto command);
    Task<bool> Send(UpdateTagCommandProto command);
}