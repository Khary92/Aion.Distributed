using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Grpc.Net.Client;
using Proto.Command.WorkDays;

namespace Client.Desktop.Communication.Commands.WorkDays;

public class WorkDayCommandSender : IWorkDayCommandSender
{
    private readonly WorkDayCommandProtoService.WorkDayCommandProtoServiceClient _client;

    public WorkDayCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new WorkDayCommandProtoService.WorkDayCommandProtoServiceClient(channel);
    }

    public async Task<bool> Send(ClientCreateWorkDayCommand command)
    {
        var response = await _client.CreateWorkDayAsync(command.ToProto());
        return response.Success;
    }
}