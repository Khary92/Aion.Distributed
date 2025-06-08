using Avalonia.ReactiveUI;
using Client.Avalonia.ViewModels.Data;

namespace Client.Avalonia.Views.Data;

public partial class DataCompositeControl : ReactiveUserControl<DataCompositeViewModel>
{
    public DataCompositeControl(DataCompositeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = ViewModel = viewModel;
    }
}