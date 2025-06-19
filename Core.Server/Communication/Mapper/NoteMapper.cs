using Google.Protobuf.WellKnownTypes;
using Proto.DTO.Note;

namespace Service.Server.Communication.Mapper;

public class NoteMapper : IDtoMapper<NoteProto, Domain.Entities.Note>
{
    public Domain.Entities.Note ToDomain(NoteProto dto)
    {
        return new Domain.Entities.Note
        {
            NoteId = Guid.Parse(dto.NoteId),
            Text = dto.Text,
            NoteTypeId = Guid.Parse(dto.NoteTypeId),
            TimeSlotId = Guid.Parse(dto.TimeSlotId),
            TimeStamp = dto.TimeStamp.ToDateTimeOffset(),
        };
    }

    public NoteProto ToDto(Domain.Entities.Note domain)
    {
        return new NoteProto
        {
            NoteId = domain.NoteId.ToString(),
            Text = domain.Text,
            NoteTypeId = domain.NoteTypeId.ToString(),
            TimeSlotId = domain.TimeSlotId.ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(domain.TimeStamp)
        };
    }
}