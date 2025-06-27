using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Client;
using Proto.Command.StatisticsData;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public class StatisticsDataCommandSender : IStatisticsDataCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateStatisticsDataCommandProto command)
    {
        var response = await _client.CreateStatisticsDataAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeTagSelectionCommandProto command)
    {
        var response = await _client.ChangeTagSelectionAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeProductivityCommandProto command)
    {
        var response = await _client.ChangeProductivityAsync(command);
        return response.Success;
    }
}