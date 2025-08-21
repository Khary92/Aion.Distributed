using Core.Server.Communication.Endpoints.UseCase.Handler;
using Grpc.Core;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Endpoints.UseCase;

public class UseCaseRequestReceiver(LoadTimeSlotControlDataHandler loadTimeSlotControlDataHandler)
    : UseCaseRequestService.UseCaseRequestServiceBase
{
    public override async Task<TimeSlotControlDataListProto> GetTimeSlotControlDataByDate(
        GetTimeSlotControlDataRequestProto request, ServerCallContext context)
    {
        return await loadTimeSlotControlDataHandler.Handle(request.ToRequest());
    }
}