using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Client;
using Proto.Requests.UseCase;

namespace Client.Desktop.Communication.Requests.UseCase;

public class UseCaseRequestSender : IUseCaseRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly UseCaseRequestService.UseCaseRequestServiceClient _client = new(Channel);

    public async Task<TimeSlotControlDataListProto> Send(GetTimeSlotControlDataRequestProto request)
    {
        return await _client.GetTimeSlotControlDataByIdAsync(request);
    }
}