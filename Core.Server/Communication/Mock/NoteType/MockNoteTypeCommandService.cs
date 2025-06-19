using Grpc.Core;
using Proto.Command.NoteTypes;
using Proto.Notifications.NoteType;
using Service.Server.Communication.NoteType;

namespace Service.Server.Communication.Mock.NoteType;

public class MockNoteTypeCommandService(NoteTypeNotificationServiceImpl noteTypeNotificationService)
    : NoteTypeCommandProtoService.NoteTypeCommandProtoServiceBase
{
    public override async Task<CommandResponse> ChangeNoteTypeColor(ChangeNoteTypeColorCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[ChangeNoteTypeColor] ID: {request.NoteTypeId}, Color: {request.Color}");

        try
        {
            await noteTypeNotificationService.SendNotificationAsync(new NoteTypeNotification
            {
                NoteTypeColorChanged = new NoteTypeColorChangedNotification
                {
                    NoteTypeId = request.NoteTypeId,
                    Color = request.Color
                }
            });

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

        try
        {
            await noteTypeNotificationService.SendNotificationAsync(new NoteTypeNotification
            {
                NoteTypeNameChanged = new NoteTypeNameChangedNotification
                {
                    NoteTypeId = request.NoteTypeId,
                    Name = request.Name
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] ChangeNoteTypeName failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> CreateNoteType(CreateNoteTypeCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateNoteType] ID: {request.NoteTypeId}, Name: {request.Name}, Color: {request.Color}");

        try
        {
            await noteTypeNotificationService.SendNotificationAsync(new NoteTypeNotification
            {
                NoteTypeCreated = new NoteTypeCreatedNotification
                {
                    NoteTypeId = request.NoteTypeId,
                    Name = request.Name,
                    Color = request.Color
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateNoteType failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}