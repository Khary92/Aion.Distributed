using MediatR;

namespace Contract.CQRS.Commands.Entities.AiSettings;

public record ChangePromptCommand(Guid AiSettingsId, string Prompt) : IRequest<Unit>, INotification;