using Core.Server.Communication.Endpoints.UseCase.Handler;
using Grpc.Core;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Endpoints.UseCase;

public class UseCaseRequestReceiver(ILoadTrackingControlDataHandler loadTrackingControlDataHandler)
    : UseCaseRequestService.UseCaseRequestServiceBase
{
    public override async Task<TrackingControlDataListProto> GetTimeSlotControlDataByDate(
        GetTimeSlotControlDataRequestProto request, ServerCallContext context)
    {
        return await loadTrackingControlDataHandler.Handle(request.ToRequest());
    }
}