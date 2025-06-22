using Core.Server.Communication.Services.UseCase.Handler;
using Grpc.Core;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Services.UseCase;

public class UseCaseRequestReceiver(LoadTimeSlotControlDataHandler loadTimeSlotControlDataHandler)
    : UseCaseRequestService.UseCaseRequestServiceBase
{
    public override async Task<TimeSlotControlDataListProto> GetTimeSlotControlDataById(
        GetTimeSlotControlDataRequestProto request, ServerCallContext context)
    {
       return await loadTimeSlotControlDataHandler.Handle(request.ToRequest());
    }
}