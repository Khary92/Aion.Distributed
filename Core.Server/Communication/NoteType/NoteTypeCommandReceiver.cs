using Grpc.Core;
using Proto.Command.NoteTypes;
using Service.Server.Communication.NoteType;
using Service.Server.Old.Services.Entities.NoteTypes;

namespace Service.Server.Communication.Mock.NoteType;

public class NoteTypeCommandReceiver(
    NoteTypeNotificationServiceImpl noteTypeNotificationService,
    INoteTypeCommandsService noteTypeCommandsService)
    : NoteTypeCommandProtoService.NoteTypeCommandProtoServiceBase
{
    public override async Task<CommandResponse> ChangeNoteTypeColor(ChangeNoteTypeColorCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangeNoteTypeColor] ID: {request.NoteTypeId}, Color: {request.Color}");

        await noteTypeCommandsService.ChangeColor(request.ToCommand());

        try
        {
            await noteTypeNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeNoteTypeColor failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> ChangeNoteTypeName(ChangeNoteTypeNameCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangeNoteTypeName] ID: {request.NoteTypeId}, Name: {request.Name}");

        await noteTypeCommandsService.ChangeName(request.ToCommand());

        try
        {
            await noteTypeNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeNoteTypeName failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> CreateNoteType(CreateNoteTypeCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[CreateNoteType] ID: {request.NoteTypeId}, Name: {request.Name}, Color: {request.Color}");

        await noteTypeCommandsService.Create(request.ToCommand());

        try
        {
            await noteTypeNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateNoteType failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}