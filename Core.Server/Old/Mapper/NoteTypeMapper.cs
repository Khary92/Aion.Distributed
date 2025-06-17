using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class NoteTypeMapper : IDtoMapper<NoteTypeDto, NoteType>
{
    public NoteType ToDomain(NoteTypeDto dto)
    {
        return new NoteType
        {
            NoteTypeId = dto.NoteTypeId,
            Name = dto.Name,
            Color = dto.Color
        };
    }

    public NoteTypeDto ToDto(NoteType domain)
    {
        return new NoteTypeDto(domain.NoteTypeId, domain.Name, domain.Color);
    }
}