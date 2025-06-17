using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class NoteMapper : IDtoMapper<NoteDto, Note>
{
    public Note ToDomain(NoteDto dto)
    {
        return new Note
        {
            NoteId = dto.NoteId,
            Text = dto.Text,
            NoteTypeId = dto.NoteTypeId,
            TimeSlotId = dto.TimeSlotId,
            TimeStamp = dto.TimeStamp
        };
    }

    public NoteDto ToDto(Note domain)
    {
        return new NoteDto(domain.NoteId, domain.Text, domain.NoteTypeId, domain.TimeSlotId, domain.TimeStamp);
    }
}