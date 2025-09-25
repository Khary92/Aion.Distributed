using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Client.Records;

namespace Client.Desktop.Communication.Requests.Client;

public interface IClientRequestSender
{
    Task<List<ClientGetTrackingControlResponse>> Send(ClientGetTrackingControlDataRequest request);
}