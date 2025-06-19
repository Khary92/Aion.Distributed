using Proto.DTO.Ticket;

namespace Service.Server.Communication.Mapper;

public class TicketMapper : IDtoMapper<TicketProto, Domain.Entities.Ticket>
{
    public Domain.Entities.Ticket ToDomain(TicketProto dto)
    {
        return new Domain.Entities.Ticket
        {
            TicketId = Guid.Parse(dto.TicketId),
            Name = dto.Name,
            BookingNumber = dto.BookingNumber,
            Documentation = dto.Documentation,
            SprintIds = dto.SprintIds.ToGuidList(),
        };
    }

    public TicketProto ToDto(Domain.Entities.Ticket domain)
    {
        return new TicketProto
        {
            TicketId = domain.TicketId.ToString(),
            Name = domain.Name,
            BookingNumber = domain.BookingNumber,
            Documentation = domain.Documentation,
            SprintIds = { domain.SprintIds.ToRepeatedField() }
        };
    }
}