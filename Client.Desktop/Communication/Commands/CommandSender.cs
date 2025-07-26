using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.WorkDays;
using Proto.Command.Notes;
using Proto.Command.NoteTypes;
using Proto.Command.Sprints;
using Proto.Command.StatisticsData;
using Proto.Command.Tags;
using Proto.Command.Tickets;
using Proto.Command.TimerSettings;
using Proto.Command.TimeSlots;
using Proto.Command.UseCases;
using Proto.Command.WorkDays;
using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Commands.Tickets;
using ITimerSettingsCommandSender = Service.Proto.Shared.Commands.TimerSettings.ITimerSettingsCommandSender;

namespace Client.Desktop.Communication.Commands;

public class CommandSender(
    INoteCommandSender noteCommandSender,
    INoteTypeCommandSender noteTypeCommandSender,
    ISprintCommandSender sprintCommandSender,
    IStatisticsDataCommandSender statisticsDataCommandSender,
    ITagCommandSender tagCommandSender,
    ITimerSettingsCommandSender timerSettingsCommandSender,
    ITimeSlotCommandSender timeSlotCommandSender,
    IUseCaseCommandSender useCaseCommandSender,
    IWorkDayCommandSender workDayCommandSender,
    ITicketCommandSender ticketCommandSender) : ICommandSender
{
    public async Task<bool> Send(CreateNoteCommandProto command)
    {
        return await noteCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateNoteCommandProto command)
    {
        return await noteCommandSender.Send(command);
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

    public async Task<bool> Send(CreateStatisticsDataCommandProto command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeTagSelectionCommandProto command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeProductivityCommandProto command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTagCommandProto command)
    {
        return await tagCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateTagCommandProto command)
    {
        return await tagCommandSender.Send(command);
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

    public async Task<bool> Send(CreateTimeSlotCommandProto command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(AddNoteCommandProto command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(SetStartTimeCommandProto command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(SetEndTimeCommandProto command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTimeSlotControlCommandProto command)
    {
        return await useCaseCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateWorkDayCommandProto command)
    {
        return await workDayCommandSender.Send(command);
    }

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
}