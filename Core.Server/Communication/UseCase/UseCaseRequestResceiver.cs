using Grpc.Core;
using Proto.Requests.UseCase;

namespace Service.Server.Communication.UseCase;

public class UseCaseRequestResceiver : UseCaseRequestService.UseCaseRequestServiceBase
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