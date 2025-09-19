using System;
using Client.Desktop.Communication.Requests.Timer;
using Proto.DTO.TraceData;
using Proto.Requests.NoteTypes;
using Proto.Requests.TimerSettings;

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

    public static GetTimerSettingsRequestProto ToProto(this ClientGetTimerSettingsRequest request)
    {
        return new GetTimerSettingsRequestProto
        {
            TraceData = new TraceDataProto
            {
                TraceId = request.TraceId.ToString()
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