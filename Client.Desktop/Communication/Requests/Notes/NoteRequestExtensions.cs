using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.DataModels;
using Proto.DTO.Note;
using Proto.DTO.TraceData;
using Proto.Requests.Notes;

namespace Client.Desktop.Communication.Requests.Notes;

public static class NoteRequestExtensions
{
    public static GetNotesByTimeSlotIdRequestProto ToProto(this ClientGetNotesByTimeSlotIdRequest request)
    {
        return new GetNotesByTimeSlotIdRequestProto
        {
            TimeSlotId = request.TimeSlotId.ToString(),
            TraceData = new TraceDataProto
            {
                TraceId = request.TraceId.ToString()
            }
        };
    }

    public static GetNotesByTicketIdRequestProto ToProto(this ClientGetNotesByTicketIdRequest request)
    {
        return new GetNotesByTicketIdRequestProto
        {
            TicketId = request.TicketId.ToString(),
            TraceData = new TraceDataProto
            {
                TraceId = request.TraceId.ToString()
            }
        };
    }

    public static List<NoteClientModel> ToClientModelList(this GetNotesResponseProto proto)
    {
        return proto.Notes.Select(ToClientModel).ToList();
    }

    private static NoteClientModel ToClientModel(this NoteProto proto)
    {
        return new NoteClientModel(Guid.Parse(proto.NoteId), proto.Text, Guid.Parse(proto.NoteTypeId),
            Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.TicketId), proto.TimeStamp.ToDateTimeOffset());
    }
}