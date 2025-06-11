using Client.Avalonia.Communication.Commands.AiSettings;
using Client.Avalonia.Communication.Commands.Notes;
using Client.Avalonia.Communication.Commands.NoteTypes;
using Client.Avalonia.Communication.Commands.Settings;
using Client.Avalonia.Communication.Commands.Sprints;
using Client.Avalonia.Communication.Commands.StatisticsData;
using Client.Avalonia.Communication.Commands.Tags;
using Client.Avalonia.Communication.Commands.Tickets;
using Client.Avalonia.Communication.Commands.TimerSettings;
using Client.Avalonia.Communication.Commands.TimeSlots;
using Client.Avalonia.Communication.Commands.TraceReports;
using Client.Avalonia.Communication.Commands.UseCases;
using Client.Avalonia.Communication.Commands.WorkDays;

namespace Client.Avalonia.Communication.Commands;

public interface ICommandSender : 
    IAiSettingsCommandSender,
    INoteCommandSender,
    INoteTypeCommandSender,
    ISettingsCommandSender, 
    ISprintCommandSender, 
    IStatisticsDataCommandSender, 
    ITagCommandSender, 
    ITicketCommandSender,
    ITimerSettingsCommandSender, 
    ITimeSlotCommandSender, 
    ITraceReportCommandSender, 
    IUseCaseCommandSender, 
    IWorkDayCommandSender;