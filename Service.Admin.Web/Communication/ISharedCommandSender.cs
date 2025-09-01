using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Commands.TimerSettings;

namespace Service.Admin.Web.Communication;

public interface ISharedCommandSender : ITicketCommandSender, ISprintCommandSender, ITagCommandSender,
    INoteTypeCommandSender, ITimerSettingsCommandSender;