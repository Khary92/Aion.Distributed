using Grpc.Core;
using Proto.Command.Notes;
using Service.Server.Old.Services.Entities.Notes;

namespace Service.Server.Communication.Note;

public class NoteCommandReceiver(
    NoteNotificationService noteNotificationService,
    INoteCommandsService noteCommandsService)
    : NoteCommandProtoService.NoteCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateNote(CreateNoteCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateNote] ID: {request.NoteId}, Text: {request.Text}, NoteTypeId: {request.NoteTypeId}, TimeSlotId: {request.TimeSlotId}, TimeStamp: {request.TimeStamp}");

        await noteCommandsService.Create(request.ToCommand());

        try
        {
            await noteNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateNote failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> UpdateNote(UpdateNoteCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateNote] ID: {request.NoteId}, Text: {request.Text}, NoteTypeId: {request.NoteTypeId}, TimeSlotId: {request.TimeSlotId}");

        await noteCommandsService.Update(request.ToCommand());

        try
        {
            await noteNotificationService.SendNotificationAsync(request.ToNotification());
            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] UpdateNote failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}