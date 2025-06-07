using MediatR;

namespace Contract.CQRS.Requests.Settings;

public record IsExportPathValidRequest : IRequest<bool>, INotification;