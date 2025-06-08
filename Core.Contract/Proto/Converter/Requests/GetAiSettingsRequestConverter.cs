using Contract.CQRS.Requests.AiSettings;
using Contract.DTO;
using proto.requests.ai_settings;

public static class AiSettingsRequestConverter
{
    public static GetAiSettingsRequestProto ToProto(GetAiSettingsRequest _)
        => new();

    public static GetAiSettingsRequest FromProto(GetAiSettingsRequestProto _)
        => new();
}