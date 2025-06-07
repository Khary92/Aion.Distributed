using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.TimerSettings;

public record GetTimerSettingsRequest : IRequest<TimerSettingsDto?>;