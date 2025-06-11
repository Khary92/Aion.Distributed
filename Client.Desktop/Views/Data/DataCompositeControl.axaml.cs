using Avalonia.ReactiveUI;
using Client.Desktop.Models.Data;

namespace Client.Desktop.Views.Data;

public partial class DataCompositeControl : ReactiveUserControl<DataCompositeViewModel>
{
    public DataCompositeControl(DataCompositeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = ViewModel = viewModel;
    }
}