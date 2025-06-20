namespace Core.Server.Communication.CQRS.Commands.Entities.AiSettings;

public record ChangePromptCommand(Guid AiSettingsId, string Prompt);