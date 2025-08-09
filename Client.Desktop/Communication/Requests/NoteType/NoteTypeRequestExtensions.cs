using System;
using Proto.DTO.TraceData;
using Proto.Requests.NoteTypes;

namespace Client.Desktop.Communication.Requests.NoteType;

public static class NoteTypeRequestExtensions
{
    public static GetAllNoteTypesRequestProto ToProto(this ClientGetAllNoteTypesRequest request)
    {
        return new GetAllNoteTypesRequestProto
        {
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };
    }


    public static GetNoteTypeByIdRequestProto ToProto(this ClientGetNoteTypeByIdRequest request)
    {
        return new GetNoteTypeByIdRequestProto
        {
            NoteTypeId = request.NoteTypeId.ToString(),
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };
    }
}