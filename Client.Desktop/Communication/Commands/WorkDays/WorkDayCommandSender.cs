using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Command.WorkDays;

namespace Client.Desktop.Communication.Commands.WorkDays;

public class WorkDayCommandSender : IWorkDayCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly WorkDayCommandProtoService.WorkDayCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(ClientCreateWorkDayCommand command)
    {
        var response = await _client.CreateWorkDayAsync(command.ToProto());
        return response.Success;
    }
}