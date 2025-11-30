using Core.Server.Services.Entities.NoteTypes;
using Core.Server.Tracing.Tracing.Tracers;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proto.Command.NoteTypes;

namespace Core.Server.Communication.Endpoints.NoteType;

[Authorize]
public class NoteTypeCommandReceiver(
    INoteTypeCommandsService noteTypeCommandsService,
    ITraceCollector tracer)
    : NoteTypeProtoCommandService.NoteTypeProtoCommandServiceBase
{
    public override async Task<CommandResponse> ChangeNoteTypeColor(ChangeNoteTypeColorCommandProto request,
        ServerCallContext context)
    {
        await tracer.NoteType.ChangeColor.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await noteTypeCommandsService.ChangeColor(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeNoteTypeName(ChangeNoteTypeNameCommandProto request,
        ServerCallContext context)
    {
        await tracer.NoteType.ChangeName.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        await noteTypeCommandsService.ChangeName(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> CreateNoteType(CreateNoteTypeCommandProto request,
        ServerCallContext context)
    {
        await tracer.NoteType.Create.CommandReceived(GetType(), Guid.Parse(request.TraceData.TraceId), request);

        Console.WriteLine($"[CreateNoteType] ID: {request.NoteTypeId}, Name: {request.Name}, Color: {request.Color}");

        await noteTypeCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}