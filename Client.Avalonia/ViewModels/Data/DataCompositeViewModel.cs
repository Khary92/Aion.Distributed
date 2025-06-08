namespace Client.Avalonia.ViewModels.Data;

public class DataCompositeViewModel(
    SprintsDataViewModel sprintsDataViewModel,
    TicketsDataViewModel ticketsDataViewModel,
    TagsDataViewModel tagsDataViewModel,
    NotesDataViewModel notesDataViewModel)
{
    public SprintsDataViewModel SprintsDataViewModel { get; } = sprintsDataViewModel;
    public TicketsDataViewModel TicketsDataViewModel { get; } = ticketsDataViewModel;
    public TagsDataViewModel TagsDataViewModel { get; } = tagsDataViewModel;
    public NotesDataViewModel NotesDataViewModel { get; } = notesDataViewModel;
}