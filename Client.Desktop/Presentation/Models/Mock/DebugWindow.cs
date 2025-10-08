namespace Client.Desktop.Presentation.Models.Mock;

public class DebugWindow(
    ServerSprintDataViewModel serverSprintDataViewModel,
    ServerTicketDataViewModel serverTicketDataViewModel,
    ServerTagDataViewModel serverTagDataViewModel,
    ServerNoteTypeDataViewModel serverNoteTypeDataViewModel)
{
    public ServerSprintDataViewModel ServerSprintDataViewModel { get; } = serverSprintDataViewModel;
    public ServerTicketDataViewModel ServerTicketDataViewModel { get; } = serverTicketDataViewModel;
    public ServerTagDataViewModel ServerTagDataViewModel { get; } = serverTagDataViewModel;
    public ServerNoteTypeDataViewModel ServerNoteTypeDataViewModel { get; } = serverNoteTypeDataViewModel;
}