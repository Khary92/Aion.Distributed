using Proto.Command.NoteTypes;
using Proto.DTO.TraceData;
using Proto.Notifications.NoteType;
using Service.Admin.Web.Communication.NoteType.Records.Commands;
using Service.Admin.Web.Communication.NoteType.Records.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.NoteType;

public static class NoteTypeExtensions
{
    public static CreateNoteTypeCommandProto ToProto(this WebCreateNoteTypeCommand command)
    {
        return new CreateNoteTypeCommandProto
        {
            NoteTypeId = command.NoteTypeId.ToString(),
            Name = command.Name,
            Color = command.Color,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static ChangeNoteTypeNameCommandProto ToProto(this WebChangeNoteTypeNameCommand command)
    {
        return new ChangeNoteTypeNameCommandProto
        {
            NoteTypeId = command.NoteTypeId.ToString(),
            Name = command.Name,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static ChangeNoteTypeColorCommandProto ToProto(this WebChangeNoteTypeColorCommand command)
    {
        return new ChangeNoteTypeColorCommandProto
        {
            NoteTypeId = command.NoteTypeId.ToString(),
            Color = command.Color,
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static NewNoteTypeMessage ToWebModel(this NoteTypeCreatedNotification notification)
    {
        return new NewNoteTypeMessage(
            new NoteTypeWebModel(
                Guid.Parse(notification.NoteTypeId), notification.Name, notification.Color),
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebNoteTypeColorChangedNotification
        ToNotification(this NoteTypeColorChangedNotification notification)
    {
        return new WebNoteTypeColorChangedNotification(
            Guid.Parse(notification.NoteTypeId), notification.Color, Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebNoteTypeNameChangedNotification
        ToNotification(this NoteTypeNameChangedNotification notification)
    {
        return new WebNoteTypeNameChangedNotification(Guid.Parse(notification.NoteTypeId),
            notification.Name, Guid.Parse(notification.TraceData.TraceId));
    }
}