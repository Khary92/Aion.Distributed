using System.Collections.ObjectModel;
using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class TicketMapper : IDtoMapper<TicketDto, Ticket>
{
    public Ticket ToDomain(TicketDto dto)
    {
        return new Ticket
        {
            TicketId = dto.TicketId,
            Name = dto.Name,
            BookingNumber = dto.BookingNumber,
            Documentation = dto.Documentation,
            SprintIds = new Collection<Guid>(dto.SprintIds.ToList())
        };
    }

    public TicketDto ToDto(Ticket domain)
    {
        return new TicketDto(domain.TicketId, domain.Name, domain.BookingNumber, domain.Documentation,
            domain.SprintIds);
    }
}