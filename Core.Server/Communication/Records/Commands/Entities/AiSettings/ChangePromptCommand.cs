namespace Core.Server.Communication.Records.Commands.Entities.AiSettings;

public record ChangePromptCommand(Guid AiSettingsId, string Prompt);