
namespace Application.Contract.CQRS.Commands.Entities.AiSettings;

public record ChangePromptCommand(Guid AiSettingsId, string Prompt);