using Service.Admin.Web.Communication.Commands.NoteTypes;
using Service.Admin.Web.Communication.Commands.Sprints;
using Service.Admin.Web.Communication.Commands.Tags;
using Service.Admin.Web.Communication.Commands.Tickets;
using Service.Admin.Web.Communication.Commands.TimerSettings;

namespace Service.Admin.Web.Communication.Sender.Common;

public interface ISharedCommandSender : ITicketCommandSender, ISprintCommandSender, ITagCommandSender,
    INoteTypeCommandSender, ITimerSettingsCommandSender;