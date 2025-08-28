using Grpc.Core;
using Proto.Command.Notes;
using Proto.Notifications.Note;
using NoteNotificationService = Core.Server.Communication.Endpoints.Note.NoteNotificationService;

namespace Core.Server.Communication.Mocks.Note;

public class MockNoteCommandReceiver(NoteNotificationService noteNotificationService)
    : NoteCommandProtoService.NoteCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateNote(CreateNoteCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateNote] ID: {request.NoteId}, Text: {request.Text}, NoteTypeId: {request.NoteTypeId}, TicketId: {request.TimeSlotId}, TimeStamp: {request.TimeStamp}");

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
                    TimeStamp = request.TimeStamp
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

    public override async Task<CommandResponse> UpdateNote(UpdateNoteCommandProto request, ServerCallContext context)
    {
        Console.WriteLine(
            $"[UpdateNote] ID: {request.NoteId}, Text: {request.Text}, NoteTypeId: {request.NoteTypeId}, TicketId: {request.TimeSlotId}");

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