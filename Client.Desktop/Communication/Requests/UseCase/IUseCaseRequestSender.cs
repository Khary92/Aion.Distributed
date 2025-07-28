using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.UseCase.Records;
using Proto.Requests.UseCase;

namespace Client.Desktop.Communication.Requests.UseCase;

public interface IUseCaseRequestSender
{
    Task<List<ClientGetTimeSlotControlResponse>> Send(ClientGetTimeSlotControlDataRequest request);
}