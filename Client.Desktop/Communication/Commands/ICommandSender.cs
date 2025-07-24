using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.Settings;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.WorkDays;
using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Commands.TimerSettings;

namespace Client.Desktop.Communication.Commands;

public interface ICommandSender :
    INoteCommandSender,
    INoteTypeCommandSender,
    ISettingsCommandSender,
    ISprintCommandSender,
    IStatisticsDataCommandSender,
    ITagCommandSender,
    ITimerSettingsCommandSender,
    ITimeSlotCommandSender,
    IUseCaseCommandSender,
    IWorkDayCommandSender,
    ITicketCommandSender;