using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.UseCase.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.UseCase;

namespace Client.Desktop.Communication.Requests.UseCase;

public class UseCaseRequestSender : IUseCaseRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly UseCaseRequestService.UseCaseRequestServiceClient _client = new(Channel);

    public async Task<List<ClientGetTimeSlotControlResponse>> Send(ClientGetTimeSlotControlDataRequest request)
    {
        var timeSlotControlDatalist = await _client.GetTimeSlotControlDataByIdAsync(request.ToProto());
        return timeSlotControlDatalist.ToResponseDataList();
    }
}