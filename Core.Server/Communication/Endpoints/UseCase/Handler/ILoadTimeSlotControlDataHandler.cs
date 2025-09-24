using Core.Server.Communication.Records.Commands.UseCase.Commands;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Endpoints.UseCase.Handler;

public interface ILoadTimeSlotControlDataHandler
{
    Task<TimeSlotControlDataListProto> Handle(GetTimeSlotControlDataForDateRequest request);
}