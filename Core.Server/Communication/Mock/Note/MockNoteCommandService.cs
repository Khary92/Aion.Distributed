using Grpc.Core;
using Proto.Command.Notes;
using Proto.Notifications.Note;

namespace Service.Server.Mock.Note;

public class MockNoteCommandService(NoteNotificationServiceImpl noteNotificationService)
    : Proto.Command.Notes.NoteCommandService.NoteCommandServiceBase
{
    public override async Task<CommandResponse> CreateNote(CreateNoteCommand request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateNote] ID: {request.NoteId}, Text: {request.Text}, NoteTypeId: {request.NoteTypeId}, TimeSlotId: {request.TimeSlotId}, TimeStamp: {request.TimeStamp}");

        try
        {
            await noteNotificationService.SendNotificationAsync(new NoteNotification
            {
                NoteCreated = new NoteCreatedNotification
                {
                    NoteId = request.NoteId,
                    Text = request.Text,
                    NoteTypeId = request.NoteTypeId,
                    TimeSlotId = request.TimeSlotId,
                    TimeStamp = request.TimeStamp.ToDateTime().ToString("o") // ISO 8601 string
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateNote failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> UpdateNote(UpdateNoteCommand request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateNote] ID: {request.NoteId}, Text: {request.Text}, NoteTypeId: {request.NoteTypeId}, TimeSlotId: {request.TimeSlotId}");

        try
        {
            await noteNotificationService.SendNotificationAsync(new NoteNotification
            {
                NoteUpdated = new NoteUpdatedNotification
                {
                    NoteId = request.NoteId,
                    Text = request.Text,
                    NoteTypeId = request.NoteTypeId,
                    TimeSlotId = request.TimeSlotId
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] UpdateNote failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}