using Application.Contract.DTO;

namespace Application.Services.Entities.StatisticsData;

public interface IStatisticsDataRequestsService
{
    Task<StatisticsDataDto> GetStatisticsDataByTimeSlotId(Guid timeSlotId);
    Task<List<StatisticsDataDto>> GetAll();
}