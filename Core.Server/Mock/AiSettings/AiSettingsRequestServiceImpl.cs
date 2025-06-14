using Grpc.Core;
using Proto.Requests.AiSettings;

namespace Service.Server.Mock.AiSettings;

public class AiSettingsRequestServiceImpl : AiSettingsRequestService.AiSettingsRequestServiceBase
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