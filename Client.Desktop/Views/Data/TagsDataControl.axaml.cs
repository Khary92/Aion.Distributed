using Avalonia.ReactiveUI;
using Client.Desktop.Models.Data;

namespace Client.Desktop.Views.Data;

public partial class TagsDataControl : ReactiveUserControl<TagsDataViewModel>
{
    public TagsDataControl()
    {
        InitializeComponent();
    }

    public TagsDataControl(TagsDataViewModel dataViewModel)
    {
        InitializeComponent();
        DataContext = dataViewModel;
    }
}