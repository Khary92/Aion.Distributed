
namespace Service.Server.Communication.CQRS.Requests.StatisticsData;

public record GetStatisticsDataByTimeSlotIdRequest(Guid TimeSlotId);