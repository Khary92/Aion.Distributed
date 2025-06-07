using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.AiSettings;

public record GetAiSettingsRequest : IRequest<AiSettingsDto>, INotification;