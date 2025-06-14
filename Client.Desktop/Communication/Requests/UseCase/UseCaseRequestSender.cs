using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Requests.UseCase;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.UseCase;

public class UseCaseRequestSender : IUseCaseRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly UseCaseRequestService.UseCaseRequestServiceClient _client = new(Channel);

    public async Task<TimeSlotControlDataProto> Send(GetTimeSlotControlDataRequestProto request)
    {
        return await _client.GetTimeSlotControlDataByIdAsync(request);
    }
}