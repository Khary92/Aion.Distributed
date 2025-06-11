using System;
using Contract.DTO;
using Proto.Requests.AiSettings;

namespace Client.Avalonia.Communication.Requests;

public static class ProtoToDtoMapper
{
    public static AiSettingsDto ToDto(this AiSettingsProto proto)
    {
        return new AiSettingsDto(Guid.Parse(proto.AiSettingsId), proto.LanguageModelPath, proto.Prompt);
    }
}