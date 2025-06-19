using Proto.DTO.NoteType;

namespace Service.Server.Communication.Mapper;

public class NoteTypeMapper : IDtoMapper<NoteTypeProto, Domain.Entities.NoteType>
{
    public Domain.Entities.NoteType ToDomain(NoteTypeProto dto)
    {
        return new Domain.Entities.NoteType
        {
            NoteTypeId = Guid.Parse(dto.NoteTypeId),
            Name = dto.Name,
            Color = dto.Color
        };
    }

    public NoteTypeProto ToDto(Domain.Entities.NoteType domain)
    {
        return new NoteTypeProto
        {
            NoteTypeId = domain.NoteTypeId.ToString(),
            Name = domain.Name,
            Color = domain.Color
        };
    }
}