using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.StatisticsData.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Command.StatisticsData;

namespace Client.Desktop.Communication.Commands.StatisticsData;

public class StatisticsDataCommandSender : IStatisticsDataCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly StatisticsDataCommandProtoService.StatisticsDataCommandProtoServiceClient _client = new(Channel);

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