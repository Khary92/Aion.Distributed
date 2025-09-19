using Proto.Command.NoteTypes;
using Proto.Command.Sprints;
using Proto.Command.Tags;
using Proto.Command.Tickets;
using Proto.Command.TimerSettings;
using Service.Admin.Web.Communication.Policies;
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
    ITimerSettingsCommandSender timerSettingsCommandSender,
    CommandSenderPolicy commandSenderPolicy)
    : ISharedCommandSender
{
    public async Task<bool> Send(CreateTicketCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            ticketCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateTicketDataCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            ticketCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            ticketCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateSprintCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(AddTicketToActiveSprintCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(SetSprintActiveStatusCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateSprintDataCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateTagCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            tagCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateTagCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            tagCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateNoteTypeCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            noteTypeCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeNoteTypeNameCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            noteTypeCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeNoteTypeColorCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            noteTypeCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateTimerSettingsCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            timerSettingsCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            timerSettingsCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command)
    {
        return await commandSenderPolicy.Policy.ExecuteAsync(() =>
            timerSettingsCommandSender.Send(command));
    }
}