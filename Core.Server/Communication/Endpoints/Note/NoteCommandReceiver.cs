using Core.Server.Services.Entities.Notes;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Proto.Command.Notes;

namespace Core.Server.Communication.Endpoints.Note;

public class NoteCommandReceiver(INoteCommandsService noteCommandsService, ITraceCollector tracer)
    : NoteCommandProtoService.NoteCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateNote(CreateNoteCommandProto request, ServerCallContext context)
    {
        await tracer.Note.Create.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await noteCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> UpdateNote(UpdateNoteCommandProto request, ServerCallContext context)
    {
        await tracer.Note.Update.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await noteCommandsService.Update(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}