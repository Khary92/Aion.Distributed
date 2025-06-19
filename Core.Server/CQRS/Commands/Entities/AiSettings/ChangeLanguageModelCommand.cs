
namespace Service.Server.CQRS.Commands.Entities.AiSettings;

public record ChangeLanguageModelCommand(Guid AiSettingsId, string LanguageModelPath);