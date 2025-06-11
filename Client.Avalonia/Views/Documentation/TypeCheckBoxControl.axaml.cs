using Avalonia.Controls;
using Client.Avalonia.Models.Documentation;
using ReactiveUI;

namespace Client.Avalonia.Views.Documentation;

public partial class TypeCheckBoxControl : UserControl, IViewFor<TypeCheckBoxViewModel>
{
    public TypeCheckBoxControl(TypeCheckBoxViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public TypeCheckBoxControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TypeCheckBoxViewModel?)value;
    }

    public TypeCheckBoxViewModel? ViewModel { get; set; }
}