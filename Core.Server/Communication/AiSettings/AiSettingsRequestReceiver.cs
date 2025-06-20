using Grpc.Core;
using Proto.DTO.AiSettings;
using Proto.Requests.AiSettings;
using Service.Server.Communication.Mapper;
using Service.Server.CQRS.Commands.Entities.AiSettings;
using Service.Server.Old.Services.Entities.AiSettings;

namespace Service.Server.Communication.AiSettings;

public class AiSettingsRequestReceiver(
    IAiSettingsRequestsService aiSettingsRequestsService)
    : AiSettingsRequestService.AiSettingsRequestServiceBase
{
    public override async Task<AiSettingsProto> GetAiSettings(GetAiSettingsRequestProto request,
        ServerCallContext context)
    {
        var aiSettings = await aiSettingsRequestsService.Get();
        return aiSettings.ToProto();
    }

    public override async Task<AiSettingExistsResponseProto> AiSettingsExists(AiSettingExistsRequestProto request,
        ServerCallContext context)
    {
        return new AiSettingExistsResponseProto
        {
            Exists = await aiSettingsRequestsService.IsAiSettingsExisting()
        };
    }
}