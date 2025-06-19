namespace Service.Server.Old.Services.Entities.StatisticsData;

public interface IStatisticsDataRequestsService
{
    Task<StatisticsDataDto> GetStatisticsDataByTimeSlotId(Guid timeSlotId);
    Task<List<StatisticsDataDto>> GetAll();
}