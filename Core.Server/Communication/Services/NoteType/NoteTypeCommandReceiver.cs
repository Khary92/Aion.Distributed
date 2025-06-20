using Grpc.Core;
using Proto.Command.NoteTypes;
using Service.Server.Services.Entities.NoteTypes;

namespace Service.Server.Communication.Services.NoteType;

public class NoteTypeCommandReceiver(
    NoteTypeNotificationService noteTypeNotificationService,
    INoteTypeCommandsService noteTypeCommandsService)
    : NoteTypeCommandProtoService.NoteTypeCommandProtoServiceBase
{
    public override async Task<CommandResponse> ChangeNoteTypeColor(ChangeNoteTypeColorCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangeNoteTypeColor] ID: {request.NoteTypeId}, Color: {request.Color}");

        await noteTypeCommandsService.ChangeColor(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> ChangeNoteTypeName(ChangeNoteTypeNameCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangeNoteTypeName] ID: {request.NoteTypeId}, Name: {request.Name}");

        await noteTypeCommandsService.ChangeName(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> CreateNoteType(CreateNoteTypeCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateNoteType] ID: {request.NoteTypeId}, Name: {request.Name}, Color: {request.Color}");

        await noteTypeCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}