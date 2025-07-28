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
    CircuitBreakerPolicy circuitBreakerPolicy)
    : ISharedCommandSender
{
    public async Task<bool> Send(CreateTicketCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            ticketCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateTicketDataCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            ticketCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            ticketCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateSprintCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(AddTicketToActiveSprintCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(AddTicketToSprintCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(SetSprintActiveStatusCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateSprintDataCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            sprintCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateTagCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            tagCommandSender.Send(command));
    }

    public async Task<bool> Send(UpdateTagCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            tagCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateNoteTypeCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            noteTypeCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeNoteTypeNameCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            noteTypeCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeNoteTypeColorCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            noteTypeCommandSender.Send(command));
    }

    public async Task<bool> Send(CreateTimerSettingsCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            timerSettingsCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            timerSettingsCommandSender.Send(command));
    }

    public async Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command)
    {
        return await circuitBreakerPolicy.Policy.ExecuteAsync(() =>
            timerSettingsCommandSender.Send(command));
    }
}