using Avalonia.Controls;
using Client.Avalonia.Models.Analysis;
using ReactiveUI;

namespace Client.Avalonia.Views.Analysis;

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