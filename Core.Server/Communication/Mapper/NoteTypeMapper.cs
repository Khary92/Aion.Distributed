using Domain.Entities;
using Proto.DTO.NoteType;

namespace Service.Server.Communication.Mapper;

public class NoteTypeMapper : IDtoMapper<NoteTypeProto, NoteType>
{
    public NoteType ToDomain(NoteTypeProto dto)
    {
        return new NoteType
        {
            NoteTypeId = Guid.Parse(dto.NoteTypeId),
            Name = dto.Name,
            Color = dto.Color
        };
    }

    public NoteTypeProto ToDto(NoteType domain)
    {
        return new NoteTypeProto
        {
            NoteTypeId = domain.NoteTypeId.ToString(),
            Name = domain.Name,
            Color = domain.Color
        };
    }
}