using System.Threading.Tasks;
using Proto.Requests.UseCase;

namespace Client.Desktop.Communication.Requests.UseCase;

public interface IUseCaseRequestSender
{
    Task<TimeSlotControlDataProto> Send(GetTimeSlotControlDataRequestProto request);
}