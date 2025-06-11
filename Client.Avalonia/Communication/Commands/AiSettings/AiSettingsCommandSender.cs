using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.AiSettings;
using Proto.Shared;

namespace Client.Avalonia.Communication.Commands.AiSettings;

public class AiSettingsCommandSender : IAiSettingsCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly AiSettingsCommandService.AiSettingsCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(ChangeLanguageModelCommand command)
    {
        var response = await _client.SendChangeLanguageModelAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangePromptCommand command)
    {
        var response = await _client.SendChangePromptAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(CreateAiSettingsCommand command)
    {
        var response = await _client.SendCreateAiSettingsAsync(command);
        return response.Success;
    }
}