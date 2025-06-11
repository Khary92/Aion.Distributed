using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.Settings;

namespace Client.Avalonia.Communication.Commands.Settings;

public class SettingsCommandSender : ISettingsCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly SettingsCommandService.SettingsCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateSettingsCommand command)
    {
        var response = await _client.CreateSettingsAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(UpdateSettingsCommand command)
    {
        var response = await _client.UpdateSettingsAsync(command);
        return response.Success;
    }
}