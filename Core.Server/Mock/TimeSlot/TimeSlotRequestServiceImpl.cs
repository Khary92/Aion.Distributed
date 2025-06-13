using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Proto.Requests.TimeSlots;

public class TimeSlotRequestServiceImpl : TimeSlotRequestService.TimeSlotRequestServiceBase
{
    public override Task<TimeSlotProto> GetTimeSlotById(GetTimeSlotByIdRequestProto request, ServerCallContext context)
    {
        var timeSlot = new TimeSlotProto
        {
            TimeSlotId = request.TimeSlotId,
            WorkDayId = "workday-123",
            SelectedTicketId = "ticket-456",
            StartTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddHours(-1).ToUniversalTime()),
            EndTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddMinutes(30).ToUniversalTime()),
            IsTimerRunning = true
        };
        timeSlot.NoteIds.Add("note-1");
        timeSlot.NoteIds.Add("note-2");

        return Task.FromResult(timeSlot);
    }

    public override Task<TimeSlotListProto> GetTimeSlotsForWorkDayId(GetTimeSlotsForWorkDayIdRequestProto request, ServerCallContext context)
    {
        var list = new TimeSlotListProto();
        list.TimeSlots.Add(new TimeSlotProto
        {
            TimeSlotId = "timeslot-1",
            WorkDayId = request.WorkDayId,
            SelectedTicketId = "ticket-1",
            StartTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddHours(-8).ToUniversalTime()),
            EndTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddHours(-7).ToUniversalTime()),
            IsTimerRunning = false
        });
        list.TimeSlots.Add(new TimeSlotProto
        {
            TimeSlotId = "timeslot-2",
            WorkDayId = request.WorkDayId,
            SelectedTicketId = "ticket-2",
            StartTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddHours(-6).ToUniversalTime()),
            EndTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddHours(-5).ToUniversalTime()),
            IsTimerRunning = true
        });

        list.TimeSlots[0].NoteIds.Add("note-1");
        list.TimeSlots[1].NoteIds.Add("note-2");

        return Task.FromResult(list);
    }
}
