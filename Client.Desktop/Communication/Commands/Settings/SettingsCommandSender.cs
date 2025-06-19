using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Settings;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.Settings;

public class SettingsCommandSender : ISettingsCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly SettingsCommandProtoService.SettingsCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateSettingsCommandProto command)
    {
        var response = await _client.CreateSettingsAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeExportPathCommandProto command)
    {
        var response = await _client.ChangeExportPathAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeAutomaticTicketAddingToSprintCommandProto command)
    {
        var response = await _client.ChangeAutomaticTicketAddingAsync(command);
        return response.Success;
    }
    
}