using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.AiSettings;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.NoteTypes;
using Client.Desktop.Communication.Commands.Settings;
using Client.Desktop.Communication.Commands.Sprints;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.Tags;
using Client.Desktop.Communication.Commands.Tickets;
using Client.Desktop.Communication.Commands.TimerSettings;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TraceReports;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.WorkDays;
using Proto.Command.AiSettings;
using Proto.Command.Notes;
using Proto.Command.NoteTypes;
using Proto.Command.Settings;
using Proto.Command.Sprints;
using Proto.Command.StatisticsData;
using Proto.Command.Tags;
using Proto.Command.Tickets;
using Proto.Command.TimerSettings;
using Proto.Command.TimeSlots;
using Proto.Command.TraceReports;
using Proto.Command.UseCases;
using Proto.Command.WorkDays;

namespace Client.Desktop.Communication.Commands;

public class CommandSender(
    IAiSettingsCommandSender aiSettingsCommandSender,
    INoteCommandSender noteCommandSender,
    INoteTypeCommandSender noteTypeCommandSender,
    ISettingsCommandSender settingsCommandSender,
    ISprintCommandSender sprintCommandSender,
    IStatisticsDataCommandSender statisticsDataCommandSender,
    ITagCommandSender tagCommandSender,
    ITicketCommandSender ticketCommandSender,
    ITimerSettingsCommandSender timerSettingsCommandSender,
    ITimeSlotCommandSender timeSlotCommandSender, ITraceReportCommandSender traceReportCommandSender, IUseCaseCommandSender useCaseCommandSender, IWorkDayCommandSender workDayCommandSender) : ICommandSender
{
    public async Task<bool> Send(ChangeLanguageModelCommand command)
    {
        return await aiSettingsCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangePromptCommand command)
    {
        return await aiSettingsCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateAiSettingsCommand command)
    {
        return await aiSettingsCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateNoteCommand command)
    {
        return await noteCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateNoteCommand command)
    {
        return await noteCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateNoteTypeCommand command)
    {
        return await noteTypeCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeNoteTypeNameCommand command)
    {
        return await noteTypeCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeNoteTypeColorCommand command)
    {
        return await noteTypeCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateSettingsCommand command)
    {
        return await settingsCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateSettingsCommand command)
    {
        return await settingsCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateSprintCommand command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(AddTicketToActiveSprintCommand command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(AddTicketToSprintCommand command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(SetSprintActiveStatusCommand command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateSprintDataCommand command)
    {
        return await sprintCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateStatisticsDataCommand command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeTagSelectionCommand command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeProductivityCommand command)
    {
        return await statisticsDataCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTagCommand command)
    {
        return await tagCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateTagCommand command)
    {
        return await tagCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTicketCommand command)
    {
        return await ticketCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateTicketDataCommand command)
    {
        return await ticketCommandSender.Send(command);
    }

    public async Task<bool> Send(UpdateTicketDocumentationCommand command)
    {
        return await ticketCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTimerSettingsCommand createTicketCommand)
    {
        return await timerSettingsCommandSender.Send(createTicketCommand);
    }

    public async Task<bool> Send(ChangeSnapshotSaveIntervalCommand command)
    {
        return await timerSettingsCommandSender.Send(command);
    }

    public async Task<bool> Send(ChangeDocuTimerSaveIntervalCommand command)
    {
        return await timerSettingsCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTimeSlotCommand command)
    {
        return await timeSlotCommandSender.Send(command);
    }

    public async Task<bool> Send(AddNoteCommand command)
    {
        return await timeSlotCommandSender.Send(command);

    }

    public async Task<bool> Send(SetStartTimeCommand command)
    {
        return await timeSlotCommandSender.Send(command);

    }

    public async Task<bool> Send(SetEndTimeCommand command)
    {
        return await timeSlotCommandSender.Send(command);

    }

    public async Task<bool> Send(SendTraceReportCommand command)
    {
        return await traceReportCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateTimeSlotControlCommand command)
    {
        return await useCaseCommandSender.Send(command);
    }

    public async Task<bool> Send(LoadTimeSlotControlCommand command)
    {
        return await useCaseCommandSender.Send(command);
    }

    public async Task<bool> Send(CreateWorkDayCommand command)
    {
        return await workDayCommandSender.Send(command);
    }
}