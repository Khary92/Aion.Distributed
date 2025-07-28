using System;
using Client.Desktop.DataModels;
using Proto.Notifications.NoteType;
using Proto.Requests.NoteTypes;

namespace Client.Desktop.Communication.Requests.NoteType;

public static class NoteTypeRequestExtensions
{
    public static GetAllNoteTypesRequestProto ToProto(this ClientGetAllNoteTypesRequest request) => new();

    public static GetNoteTypeByIdRequestProto ToProto(this ClientGetNoteTypeByIdRequest request) => new()
    {
        NoteTypeId = request.NoteTypeId.ToString()
    };
    
    public static NoteTypeClientModel ToClientModel(this NoteTypeCreatedNotification notification)
    {
        return new NoteTypeClientModel(Guid.Parse(notification.NoteTypeId), notification.Name, notification.Color);
    }
}