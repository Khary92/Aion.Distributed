using MediatR;

namespace Contract.CQRS.Requests.TimerSettings;

public record IsTimerSettingExistingRequest : IRequest<bool>;