
namespace Application.Contract.CQRS.Requests.StatisticsData;

public record GetStatisticsDataByTimeSlotIdRequest(Guid TimeSlotId);