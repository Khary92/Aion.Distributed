using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.StatisticsData;

namespace Client.Avalonia.Communication.Sender;

public class StatisticsDataCommandSender : IStatisticsDataCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly StatisticsDataCommandService.StatisticsDataCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateStatisticsDataCommand command)
    {
        var response = await _client.CreateStatisticsDataAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeTagSelectionCommand command)
    {
        var response = await _client.ChangeTagSelectionAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeProductivityCommand command)
    {
        var response = await _client.ChangeProductivityAsync(command);
        return response.Success;
    }
}