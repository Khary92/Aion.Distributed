using Avalonia.ReactiveUI;
using Client.Avalonia.Models.Data;

namespace Client.Avalonia.Views.Data;

public partial class DataCompositeControl : ReactiveUserControl<DataCompositeViewModel>
{
    public DataCompositeControl(DataCompositeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = ViewModel = viewModel;
    }
}