using Avalonia.Controls;
using Client.Desktop.Presentation.Models.Authentication;
using ReactiveUI;

namespace Client.Desktop.Presentation.Views.Authentication;

public partial class AuthenticationControl : UserControl, IViewFor<AuthenticationViewModel>
{
    public AuthenticationControl(AuthenticationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public AuthenticationControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (AuthenticationViewModel)value!;
    }

    public AuthenticationViewModel? ViewModel { get; set; }
}