using Grpc.Core;
using Proto.Command.TimeSlots;
using Proto.Notifications.TimeSlots;
using Service.Server.Communication.TimeSlot;

namespace Service.Server.Communication.Mock.TimeSlot;

public class MockTimeSlotCommandService(TimeSlotNotificationServiceImpl timeSlotNotificationService)
    : TimeSlotCommandProtoService.TimeSlotCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTimeSlot(CreateTimeSlotCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateTimeSlot] ID: {request.TimeSlotId}, TicketID: {request.SelectedTicketId}, WorkDayID: {request.WorkDayId}, StartTime: {request.StartTime}, EndTime: {request.EndTime}, TimerRunning: {request.IsTimerRunning}");

        try
        {
            await timeSlotNotificationService.SendNotificationAsync(new TimeSlotNotification
            {
                TimeSlotCreated = new TimeSlotCreatedNotification
                {
                    TimeSlotId = request.TimeSlotId,
                    SelectedTicketId = request.SelectedTicketId,
                    WorkDayId = request.WorkDayId,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    IsTimerRunning = request.IsTimerRunning
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateTimeSlot failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> AddNote(AddNoteCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[AddNote] TimeSlotID: {request.TimeSlotId}, NoteID: {request.NoteId}");

        try
        {
            await timeSlotNotificationService.SendNotificationAsync(new TimeSlotNotification
            {
                NoteAddedToTimeSlot = new NoteAddedToTimeSlotNotification
                {
                    TimeSlotId = request.TimeSlotId,
                    NoteId = request.NoteId
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] AddNote failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> SetStartTime(SetStartTimeCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[SetStartTime] TimeSlotID: {request.TimeSlotId}, Time: {request.Time}");

        try
        {
            await timeSlotNotificationService.SendNotificationAsync(new TimeSlotNotification
            {
                StartTimeSet = new StartTimeSetNotification
                {
                    TimeSlotId = request.TimeSlotId,
                    Time = request.Time
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] SetStartTime failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> SetEndTime(SetEndTimeCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[SetEndTime] TimeSlotID: {request.TimeSlotId}, Time: {request.Time}");

        try
        {
            await timeSlotNotificationService.SendNotificationAsync(new TimeSlotNotification
            {
                EndTimeSet = new EndTimeSetNotification
                {
                    TimeSlotId = request.TimeSlotId,
                    Time = request.Time
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] SetEndTime failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}
