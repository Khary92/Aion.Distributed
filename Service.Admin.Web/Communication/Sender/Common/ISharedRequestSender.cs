using Service.Admin.Web.Communication.Requests.NoteTypes;
using Service.Admin.Web.Communication.Requests.Sprints;
using Service.Admin.Web.Communication.Requests.Tags;
using Service.Admin.Web.Communication.Requests.Tickets;
using Service.Admin.Web.Communication.Requests.TimerSettings;

namespace Service.Admin.Web.Communication.Sender.Common;

public interface ISharedRequestSender : ITicketRequestSender, ISprintRequestSender, ITagRequestSender,
    INoteTypeRequestSender, ITimerSettingsRequestSender;