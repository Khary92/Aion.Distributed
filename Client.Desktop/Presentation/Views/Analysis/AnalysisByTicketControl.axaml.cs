using Avalonia.Controls;
using Client.Desktop.Presentation.Models.Analysis;
using ReactiveUI;

namespace Client.Desktop.Presentation.Views.Analysis;

public partial class AnalysisByTicketControl : UserControl, IViewFor<AnalysisByTicketViewModel>
{
    public AnalysisByTicketControl(AnalysisByTicketViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public AnalysisByTicketControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (AnalysisByTicketViewModel)value!;
    }

    public AnalysisByTicketViewModel? ViewModel { get; set; }
}