namespace Client.Avalonia.Communication.Commands;

public interface ICommandSender : IAiSettingsCommandSender, INoteCommandSender, INoteTypeCommandSender,
    ISettingsCommandSender, ISprintCommandSender, IStatisticsDataCommandSender, ITagCommandSender, ITicketCommandSender,
    ITimerSettingsCommandSender, ITimeSlotCommandSender, ITraceReportCommandSender, IUseCaseCommandSender, IWorkDayCommandSender
{
}