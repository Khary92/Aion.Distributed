using Avalonia.Controls;
using Client.Desktop.Models.Export;

namespace Client.Desktop.Views.Export;

public partial class ExportControl : UserControl
{
    public ExportControl(ExportViewModel exportViewModel)
    {
        InitializeComponent();
        DataContext = exportViewModel;
    }
}