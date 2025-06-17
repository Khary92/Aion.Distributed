
namespace Application.Contract.CQRS.Commands.Entities.AiSettings;

public record ChangeLanguageModelCommand(Guid AiSettingsId, string LanguageModelPath);