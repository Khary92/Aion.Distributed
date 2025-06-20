using Grpc.Core;
using Proto.DTO.AiSettings;
using Proto.Requests.AiSettings;

namespace Service.Server.Communication.Mock.AiSettings;

public class MockAiSettingsRequestReceiver : AiSettingsRequestService.AiSettingsRequestServiceBase
{
    public override Task<AiSettingsProto> GetAiSettings(GetAiSettingsRequestProto request, ServerCallContext context)
    {
        var response = new AiSettingsProto
        {
            AiSettingsId = Guid.NewGuid().ToString(),
            LanguageModelPath = "/models/example-model.bin",
            Prompt = "Default prompt for mock"
        };

        return Task.FromResult(response);
    }
    
    public override Task<AiSettingExistsResponseProto> AiSettingsExists(AiSettingExistsRequestProto request, ServerCallContext context)
    {
        var response = new AiSettingExistsResponseProto
        {
            Exists = false
        };

        return Task.FromResult(response);
    }
}