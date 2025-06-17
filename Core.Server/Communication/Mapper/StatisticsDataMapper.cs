using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class StatisticsDataMapper : IDtoMapper<StatisticsDataDto, StatisticsData>
{
    public StatisticsData ToDomain(StatisticsDataDto dto)
    {
        return new StatisticsData
        {
            StatisticsId = dto.StatisticsId,
            TimeSlotId = dto.TimeSlotId,
            IsProductive = dto.IsProductive,
            IsNeutral = dto.IsNeutral,
            IsUnproductive = dto.IsUnproductive,
            TagIds = dto.TagIds
        };
    }

    public StatisticsDataDto ToDto(StatisticsData domain)
    {
        return new StatisticsDataDto(domain.StatisticsId, domain.TimeSlotId, domain.TagIds, domain.IsProductive,
            domain.IsNeutral, domain.IsUnproductive);
    }
}