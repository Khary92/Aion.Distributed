using Core.Server.Services.Entities.Tags;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.Tags;

namespace Core.Server.Communication.Endpoints.Tag;

public class TagCommandReceiver(ITagCommandsService tagCommandsService, ITraceCollector tracer)
    : TagCommandProtoService.TagCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTag(CreateTagCommandProto request, ServerCallContext context)
    {
        await tracer.Tag.Create.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);
        
        await tagCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateTag(UpdateTagCommandProto request, ServerCallContext context)
    {
        await tracer.Tag.Update.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await tagCommandsService.Update(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}