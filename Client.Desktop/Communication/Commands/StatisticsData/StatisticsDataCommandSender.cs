using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Grpc.Net.Client;
using Proto.Command.StatisticsData;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public class StatisticsDataCommandSender : IStatisticsDataCommandSender
{
    private readonly StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceClient _client;

    public StatisticsDataCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceClient(channel);
    }

    public async Task<bool> Send(ClientChangeTagSelectionCommand command)
    {
        var response = await _client.ChangeTagSelectionAsync(command.ToProto());
        return response.Success;
    }

    public async Task<bool> Send(ClientChangeProductivityCommand command)
    {
        var response = await _client.ChangeProductivityAsync(command.ToProto());
        return response.Success;
    }
}