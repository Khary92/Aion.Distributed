using MediatR;

namespace Contract.CQRS.Commands.Entities.AiSettings;

public record ChangeLanguageModelCommand(Guid AiSettingsId, string LanguageModelPath) : IRequest<Unit>, INotification;