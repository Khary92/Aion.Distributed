using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Proto.DTO.StatisticsData;
using Proto.DTO.Ticket;
using Proto.DTO.TimeSlots;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Mocks.UseCase;

public class MockUseCaseRequestService : UseCaseRequestService.UseCaseRequestServiceBase
{
    public override Task<TimeSlotControlDataListProto> GetTimeSlotControlDataById(
        GetTimeSlotControlDataRequestProto request, ServerCallContext context)
    {
        var statisticsDataProto = new StatisticsDataProto
        {
            StatisticsId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString(),
            IsProductive = true,
            IsNeutral = false,
            IsUnproductive = false
        };
        statisticsDataProto.TagIds.Add(Guid.NewGuid().ToString());
        statisticsDataProto.TagIds.Add(Guid.NewGuid().ToString());

        var ticketProto = new TicketProto
        {
            TicketId = MockIds.TicketId1,
            Name = "Bugfix B",
            BookingNumber = "BN-002",
            Documentation = "Docs for Bugfix B",
            SprintIds = { Guid.NewGuid().ToString() }
        };

        var timeSlotProto = new TimeSlotProto
        {
            TimeSlotId = Guid.NewGuid().ToString(),
            WorkDayId = Guid.NewGuid().ToString(),
            SelectedTicketId = MockIds.TicketId1,
            StartTime = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(-1).ToUniversalTime()),
            EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddMinutes(30).ToUniversalTime()),
            IsTimerRunning = true
        };

        var timeSlotData = new TimeSlotControlDataProto
        {
            StatisticsDataProto = statisticsDataProto,
            TicketProto = ticketProto,
            TimeSlotProto = timeSlotProto
        };

        return Task.FromResult(new TimeSlotControlDataListProto
        {
            TimeSlotControlData = { timeSlotData }
        });
    }
}