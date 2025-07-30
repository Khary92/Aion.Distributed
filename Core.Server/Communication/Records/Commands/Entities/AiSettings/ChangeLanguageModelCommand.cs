namespace Core.Server.Communication.Records.Commands.Entities.AiSettings;

public record ChangeLanguageModelCommand(Guid AiSettingsId, string LanguageModelPath, Guid TraceId);