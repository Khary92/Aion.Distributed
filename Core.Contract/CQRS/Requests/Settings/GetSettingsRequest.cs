using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Settings;

public record GetSettingsRequest : IRequest<SettingsDto?>, INotification;