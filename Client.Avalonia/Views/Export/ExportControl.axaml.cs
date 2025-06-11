using Avalonia.Controls;
using Client.Avalonia.Models.Export;

namespace Client.Avalonia.Views.Export;

public partial class ExportControl : UserControl
{
    public ExportControl(ExportViewModel exportViewModel)
    {
        InitializeComponent();
        DataContext = exportViewModel;
    }
}