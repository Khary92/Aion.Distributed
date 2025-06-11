using Avalonia.ReactiveUI;
using Client.Avalonia.Models.Data;

namespace Client.Avalonia.Views.Data;

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