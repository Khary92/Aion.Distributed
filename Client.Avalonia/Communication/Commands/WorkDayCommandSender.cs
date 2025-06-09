using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.WorkDays;

namespace Client.Avalonia.Communication.Sender;

public class WorkDayCommandSender : IWorkDayCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly WorkDayCommandService.WorkDayCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateWorkDayCommand command)
    {
        var response = await _client.CreateWorkDayAsync(command);
        return response.Success;
    }
}