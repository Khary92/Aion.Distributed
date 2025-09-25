using Core.Server.Communication.Endpoints.Client.Handler;
using Grpc.Core;
using Proto.Requests.Client;

namespace Core.Server.Communication.Endpoints.Client;

public class UseCaseRequestReceiver(ILoadTrackingControlDataHandler loadTrackingControlDataHandler)
    : ClientRequestService.ClientRequestServiceBase
{
    public override async Task<TrackingControlDataListProto> GetTrackingControlDataByDate(
        GetTrackingControlDataRequestProto request, ServerCallContext context)
    {
        return await loadTrackingControlDataHandler.Handle(request.ToRequest());
    }
}