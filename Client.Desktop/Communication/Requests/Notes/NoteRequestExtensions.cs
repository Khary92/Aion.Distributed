using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;
using Proto.DTO.Note;
using Proto.Requests.Notes;

namespace Client.Desktop.Communication.Requests.Notes;

public static class NoteRequestExtensions
{
    public static GetNotesByTimeSlotIdRequestProto ToProto(this ClientGetNotesByTimeSlotIdRequest request) => new()
    {
        TimeSlotId = request.TimeSlotId.ToString()
    };

    public static GetNotesByTicketIdRequestProto ToProto(this ClientGetNotesByTicketIdRequest request) => new()
    {
        TicketId = request.TicketId.ToString()
    };

    public static List<NoteClientModel> ToClientModelList(this GetNotesResponseProto proto) =>
        proto.Notes.Select(ToClientModel).ToList();
    
    private static NoteClientModel ToClientModel(this NoteProto proto) => new(Guid.Parse(proto.NoteId), proto.Text,
        Guid.Parse(proto.NoteTypeId), Guid.Parse(proto.TimeSlotId), proto.TimeStamp.ToDateTimeOffset());
}