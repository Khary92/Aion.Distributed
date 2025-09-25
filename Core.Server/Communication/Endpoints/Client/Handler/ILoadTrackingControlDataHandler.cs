using Core.Server.Communication.Records.Commands.UseCase.Commands;
using Proto.Requests.Client;

namespace Core.Server.Communication.Endpoints.Client.Handler;

public interface ILoadTrackingControlDataHandler
{
    Task<TrackingControlDataListProto> Handle(GetTrackingControlDataForDateRequest request);
}