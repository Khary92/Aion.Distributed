using Avalonia.ReactiveUI;
using Client.Desktop.Models.Data;

namespace Client.Desktop.Views.Data;

public partial class NoteDataControl : ReactiveUserControl<NotesDataViewModel>
{
    public NoteDataControl()
    {
        InitializeComponent();
    }

    public NoteDataControl(NotesDataViewModel dataViewModel)
    {
        InitializeComponent();
        DataContext = dataViewModel;
    }
}