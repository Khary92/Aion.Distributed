using Grpc.Core;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Mock.UseCase;

public class MockUseCaseRequestService : UseCaseRequestService.UseCaseRequestServiceBase
{
    public override Task<TimeSlotControlDataProto> GetTimeSlotControlDataById(
        GetTimeSlotControlDataRequestProto request, ServerCallContext context)
    {
        var timeSlotData = new TimeSlotControlDataProto
        {
            StatisticsDataId = Guid.NewGuid().ToString(),
            TicketId = Guid.NewGuid().ToString(),
            TimeSlotId = Guid.NewGuid().ToString()
        };

        return Task.FromResult(timeSlotData);
    }
}