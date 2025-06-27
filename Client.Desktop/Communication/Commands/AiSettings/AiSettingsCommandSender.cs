using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Client;
using Proto.Command.AiSettings;

namespace Client.Desktop.Communication.Commands.AiSettings;

public class AiSettingsCommandSender : IAiSettingsCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly AiSettingsCommandProtoService.AiSettingsCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(ChangeLanguageModelCommandProto command)
    {
        var response = await _client.SendChangeLanguageModelAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangePromptCommandProto command)
    {
        var response = await _client.SendChangePromptAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(CreateAiSettingsCommandProto command)
    {
        var response = await _client.SendCreateAiSettingsAsync(command);
        return response.Success;
    }
}