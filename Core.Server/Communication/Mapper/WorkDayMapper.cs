using Google.Protobuf.WellKnownTypes;
using Proto.DTO.TimerSettings;

namespace Service.Server.Communication.Mapper;

public class WorkDayMapper : IDtoMapper<WorkDayProto, Domain.Entities.WorkDay>
{
    public Domain.Entities.WorkDay ToDomain(WorkDayProto dto)
    {
        return new Domain.Entities.WorkDay
        {
            WorkDayId = Guid.Parse(dto.WorkDayId),
            Date = dto.Date.ToDateTimeOffset(),
        };
    }

    public WorkDayProto ToDto(Domain.Entities.WorkDay domain)
    {
        return new WorkDayProto
        {
            WorkDayId = domain.WorkDayId.ToString(),
            Date = Timestamp.FromDateTimeOffset(domain.Date)
        };
    }
}