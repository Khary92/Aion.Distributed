
namespace Service.Server.CQRS.Commands.Entities.AiSettings;

public record ChangePromptCommand(Guid AiSettingsId, string Prompt);