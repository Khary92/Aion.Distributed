using Grpc.Core;
using Proto.Requests.AiSettings;

public class AiSettingsRequestServiceImpl : AiSettingsRequestService.AiSettingsRequestServiceBase
{
    public override Task<AiSettingsProto> GetAiSettings(GetAiSettingsRequestProto request, ServerCallContext context)
    {
        var response = new AiSettingsProto
        {
            AiSettingsId = request.AiSettingsId,
            LanguageModelPath = "/models/example-model.bin",
            Prompt = "Default prompt for " + request.AiSettingsId
        };

        return Task.FromResult(response);
    }
}