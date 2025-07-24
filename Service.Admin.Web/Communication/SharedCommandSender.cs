using Proto.Command.NoteTypes;
using Proto.Command.Sprints;
using Proto.Command.Tags;
using Proto.Command.Tickets;
using Proto.Command.TimerSettings;
using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Commands.TimerSettings;

namespace Service.Admin.Web.Communication;

public class SharedCommandSender(
    ITicketCommandSender ticketCommandSender,
    ISprintCommandSender sprintCommandSender,
    ITagCommandSender tagCommandSender,
    INoteTypeCommandSender noteTypeCommandSender,
    ITimerSettingsCommandSender timerSettingsCommandSender)
    : ISharedCommandSender
{
    public async Task<bool> Send(CreateTicketCommandProto command)
    {
        return await ticketCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateTicketDataCommandProto command)
    {
        return await ticketCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommandProto command)
    {
        return await ticketCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateSprintCommandProto command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(AddTicketToActiveSprintCommandProto command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(AddTicketToSprintCommandProto command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(SetSprintActiveStatusCommandProto command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateSprintDataCommandProto command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTagCommandProto command)
    {
        return await tagCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateTagCommandProto command)
    {
        return await tagCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateNoteTypeCommandProto command)
    {
        return await noteTypeCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeNoteTypeNameCommandProto command)
    {
        return await noteTypeCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeNoteTypeColorCommandProto command)
    {
        return await noteTypeCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTimerSettingsCommandProto command)
    {
        return await timerSettingsCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command)
    {
        return await timerSettingsCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command)
    {
        return await timerSettingsCommandSender.Send(command);
    }
}