using Core.Server.Services.Entities.Tags;
using Grpc.Core;
using Proto.Command.Tags;

namespace Core.Server.Communication.Services.Tag;

public class TagCommandReceiver(ITagCommandsService tagCommandsService)
    : TagCommandProtoService.TagCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTag(CreateTagCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateTag] ID: {request.TagId}, Name: {request.Name}");

        await tagCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTag(UpdateTagCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[UpdateTag] ID: {request.TagId}, Name: {request.Name}");

        await tagCommandsService.Update(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}