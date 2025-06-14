using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Proto.Requests.TimeSlots;

public class MockTimeSlotRequestService : TimeSlotRequestService.TimeSlotRequestServiceBase
{
    public override Task<TimeSlotProto> GetTimeSlotById(GetTimeSlotByIdRequestProto request, ServerCallContext context)
    {
        var timeSlot = new TimeSlotProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            WorkDayId = Guid.NewGuid().ToString(),
            SelectedTicketId = Guid.NewGuid().ToString(),
            StartTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddHours(-1).ToUniversalTime()),
            EndTime = Timestamp.FromDateTime(System.DateTime.UtcNow.AddMinutes(30).ToUniversalTime()),
            IsTimerRunning = true
        };
        timeSlot.NoteIds.Add(Guid.NewGuid().ToString());
        timeSlot.NoteIds.Add(Guid.NewGuid().ToString());

        return Task.FromResult(timeSlot);
    }

    public override Task<TimeSlotListProto> GetTimeSlotsForWorkDayId(GetTimeSlotsForWorkDayIdRequestProto request, ServerCallContext context)
    {
        var list = new TimeSlotListProto();
        list.TimeSlots.Add(new TimeSlotProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            WorkDayId = Guid.NewGuid().ToString(),
            SelectedTicketId = Guid.NewGuid().ToString(),
            StartTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-8).ToUniversalTime()),
            EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-7).ToUniversalTime()),
            IsTimerRunning = false
        });
        list.TimeSlots.Add(new TimeSlotProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            WorkDayId = Guid.NewGuid().ToString(),
            SelectedTicketId = Guid.NewGuid().ToString(),
            StartTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-6).ToUniversalTime()),
            EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-5).ToUniversalTime()),
            IsTimerRunning = true
        });

        list.TimeSlots[0].NoteIds.Add(Guid.NewGuid().ToString());
        list.TimeSlots[1].NoteIds.Add(Guid.NewGuid().ToString());

        return Task.FromResult(list);
    }
}
