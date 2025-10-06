using Avalonia.Controls;
using Client.Desktop.Presentation.Models.Mock;

namespace Client.Desktop.Presentation.Views.Mock;

public partial class DataCompositeControl : Window
{
    public DataCompositeControl(DataCompositeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public DataCompositeControl()
    {
        InitializeComponent();
    }
}