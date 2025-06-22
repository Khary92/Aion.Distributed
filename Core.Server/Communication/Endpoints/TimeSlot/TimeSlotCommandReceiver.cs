using Core.Server.Services.Entities.TimeSlots;
using Grpc.Core;
using Proto.Command.TimeSlots;

namespace Core.Server.Communication.Endpoints.TimeSlot;

public class TimeSlotCommandReceiver(ITimeSlotCommandsService timeSlotCommandsService)
    : TimeSlotCommandProtoService.TimeSlotCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTimeSlot(CreateTimeSlotCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine(
            $"[CreateTimeSlot] ID: {request.TimeSlotId}, TicketID: {request.SelectedTicketId}, WorkDayID: {request.WorkDayId}, StartTime: {request.StartTime}, EndTime: {request.EndTime}, TimerRunning: {request.IsTimerRunning}");

        await timeSlotCommandsService.Create(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> AddNote(AddNoteCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[AddNote] TimeSlotID: {request.TimeSlotId}, NoteID: {request.NoteId}");

        await timeSlotCommandsService.AddNote(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SetStartTime(SetStartTimeCommandProto request,
        ServerCallContext context)
    {
        Console.WriteLine($"[SetStartTime] TimeSlotID: {request.TimeSlotId}, Time: {request.Time}");

        await timeSlotCommandsService.SetStartTime(request.ToCommand());
        return new CommandResponse { Success = true };
    }

    public override async Task<CommandResponse> SetEndTime(SetEndTimeCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[SetEndTime] TimeSlotID: {request.TimeSlotId}, Time: {request.Time}");

        await timeSlotCommandsService.SetEndTime(request.ToCommand());
        return new CommandResponse { Success = true };
    }
}