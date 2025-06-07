using MediatR;

namespace Contract.CQRS.Commands.Entities.StatisticsData;

public record ChangeTagSelectionCommand(Guid StatisticsDataId, List<Guid> SelectedTagIds)
    : INotification, IRequest<Unit>;