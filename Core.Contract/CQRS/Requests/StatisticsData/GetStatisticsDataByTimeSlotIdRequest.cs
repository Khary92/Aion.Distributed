using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.StatisticsData;

public record GetStatisticsDataByTimeSlotIdRequest(Guid TimeSlotId) : IRequest<StatisticsDataDto>, INotification;