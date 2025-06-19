using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Proto.DTO.Sprint;

namespace Service.Server.Communication.Mapper;

public class SprintMapper : IDtoMapper<SprintProto, Domain.Entities.Sprint>
{
    public Domain.Entities.Sprint ToDomain(SprintProto dto)
    {
        return new Domain.Entities.Sprint
        {
            SprintId = Guid.Parse( dto.SprintId),
            Name = dto.Name,
            StartDate = dto.Start.ToDateTimeOffset(),
            EndDate = dto.End.ToDateTimeOffset(),
            IsActive = dto.IsActive,
            TicketIds = dto.TicketIds.ToGuidList()
        };
    }

    public SprintProto ToDto(Domain.Entities.Sprint domain)
    {
       return new SprintProto
       {
           SprintId = domain.SprintId.ToString(),
           Name = domain.Name,
           Start = Timestamp.FromDateTimeOffset(domain.StartDate),
           End = Timestamp.FromDateTimeOffset(domain.EndDate),
           IsActive = domain.IsActive,
           TicketIds = { domain.TicketIds.ToRepeatedField() }

       };
    }
}