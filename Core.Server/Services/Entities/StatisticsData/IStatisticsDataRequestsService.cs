namespace Service.Server.Services.Entities.StatisticsData;

public interface IStatisticsDataRequestsService
{
    Task<Domain.Entities.StatisticsData> GetStatisticsDataByTimeSlotId(Guid timeSlotId);
    Task<List<Domain.Entities.StatisticsData>> GetAll();
}