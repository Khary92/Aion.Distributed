using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class SprintMapper : IDtoMapper<SprintDto, Sprint>
{
    public Sprint ToDomain(SprintDto dto)
    {
        return new Sprint
        {
            SprintId = dto.SprintId,
            Name = dto.Name,
            StartDate = dto.StartTime,
            EndDate = dto.EndTime,
            IsActive = dto.IsActive,
            TicketIds = dto.TicketIds
        };
    }

    public SprintDto ToDto(Sprint domain)
    {
        return new SprintDto(domain.SprintId, domain.Name, domain.IsActive, domain.StartDate, domain.EndDate,
            domain.TicketIds);
    }
}