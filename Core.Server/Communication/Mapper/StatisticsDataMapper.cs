using Proto.DTO.StatisticsData;

namespace Service.Server.Communication.Mapper;

public class StatisticsDataMapper : IDtoMapper<StatisticsDataProto, Domain.Entities.StatisticsData>
{
    public Domain.Entities.StatisticsData ToDomain(StatisticsDataProto dto)
    {
        return new Domain.Entities.StatisticsData
        {
            StatisticsId = Guid.Parse(dto.StatisticsId),
            TimeSlotId = Guid.Parse(dto.TimeSlotId),
            IsProductive = dto.IsProductive,
            IsNeutral = dto.IsNeutral,
            IsUnproductive = dto.IsUnproductive,
            TagIds = dto.TagIds.ToGuidList()
        };
    }

    public StatisticsDataProto ToDto(Domain.Entities.StatisticsData domain)
    {
        return new StatisticsDataProto
        {
            StatisticsId = domain.StatisticsId.ToString(),
            TimeSlotId = domain.TimeSlotId.ToString(),
            IsProductive = domain.IsProductive,
            IsNeutral = domain.IsNeutral,
            IsUnproductive = domain.IsUnproductive,
            TagIds = { domain.TagIds.ToRepeatedField() }
        };
    }
}