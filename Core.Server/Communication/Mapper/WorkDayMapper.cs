using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class WorkDayMapper : IDtoMapper<WorkDayDto, WorkDay>
{
    public WorkDay ToDomain(WorkDayDto dto)
    {
        return new WorkDay
        {
            WorkDayId = dto.WorkDayId,
            Date = dto.Date
        };
    }

    public WorkDayDto ToDto(WorkDay domain)
    {
        return new WorkDayDto(domain.WorkDayId, domain.Date);
    }
}