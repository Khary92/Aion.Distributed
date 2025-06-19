using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.WorkDays;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.WorkDays;

public class WorkDayCommandSender : IWorkDayCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly WorkDayCommandProtoService.WorkDayCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateWorkDayCommandProto command)
    {
        var response = await _client.CreateWorkDayAsync(command);
        return response.Success;
    }
}