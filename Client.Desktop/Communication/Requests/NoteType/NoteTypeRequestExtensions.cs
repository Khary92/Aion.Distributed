using System;
using Client.Desktop.DataModels;
using Proto.Notifications.NoteType;
using Proto.Requests.NoteTypes;

namespace Client.Desktop.Communication.Requests.NoteType;

public static class NoteTypeRequestExtensions
{
    public static GetAllNoteTypesRequestProto ToProto(this ClientGetAllNoteTypesRequest request) => new()
    {
        TraceData = new()
        {
            TraceId = Guid.NewGuid().ToString()
        }
    };
    

    public static GetNoteTypeByIdRequestProto ToProto(this ClientGetNoteTypeByIdRequest request) => new()
    {
        NoteTypeId = request.NoteTypeId.ToString(),
        TraceData = new()
        {
            TraceId = Guid.NewGuid().ToString()
        }
    };
    }