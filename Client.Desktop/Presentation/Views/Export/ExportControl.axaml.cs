using Avalonia.Controls;
using Client.Desktop.Presentation.Models.Export;

namespace Client.Desktop.Presentation.Views.Export;

public partial class ExportControl : UserControl
{
    public ExportControl(ExportViewModel exportViewModel)
    {
        InitializeComponent();
        DataContext = exportViewModel;
    }
}