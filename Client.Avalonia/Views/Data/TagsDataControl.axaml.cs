using Avalonia.ReactiveUI;
using Client.Avalonia.ViewModels.Data;

namespace Client.Avalonia.Views.Data;

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