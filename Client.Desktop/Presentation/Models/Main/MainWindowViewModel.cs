using System.Threading.Tasks;
using Avalonia.Controls;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Presentation.Views.Analysis;
using Client.Desktop.Presentation.Views.Authentication;
using Client.Desktop.Presentation.Views.Documentation;
using Client.Desktop.Presentation.Views.Export;
using Client.Desktop.Presentation.Views.Setting;
using Client.Desktop.Presentation.Views.Tracking;
using Client.Desktop.Services.Authentication;
using ReactiveUI;
using Unit = System.Reactive.Unit;


namespace Client.Desktop.Presentation.Models.Main;

public class MainWindowViewModel : ReactiveObject, IEventRegistration
{
    private const int ZeroConstant = 0;
    private const int MenuTransitionDelay = 250;
    private const int MaxMenuWidth = 200;

    private readonly TrackingWrapperControl _timeTrackingControl;
    private Control _currentControl;
    private readonly ITokenService _tokenService;
    private bool _isAuthenticated;

    private bool _isMenuOpen;

    private int _menuWidth;

    public MainWindowViewModel(SettingsCompositeControl settingsCompositeControl,
        TrackingWrapperControl timeTrackingControl, ExportControl exportControl,
        AnalysisControlWrapper analysisControlWrapper, DocumentationControl documentationControl,
        AuthenticationControl authenticationControl, ITokenService tokenService)
    {
        OnSettingsClickCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentControl = settingsCompositeControl;
            await Task.Delay(MenuTransitionDelay);
            IsMenuOpen = false;
            MenuWidth = ZeroConstant;
        });

        OnTrackingClickCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentControl = timeTrackingControl;
            await Task.Delay(MenuTransitionDelay);
            IsMenuOpen = false;
            MenuWidth = ZeroConstant;
        });

        OnExportClickCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentControl = exportControl;
            await Task.Delay(MenuTransitionDelay);
            IsMenuOpen = false;
            MenuWidth = ZeroConstant;
        });

        OnAnalysisClickCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentControl = analysisControlWrapper;
            await Task.Delay(MenuTransitionDelay);
            IsMenuOpen = false;
            MenuWidth = ZeroConstant;
        });

        OnDocumentationClickCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            CurrentControl = documentationControl;
            await Task.Delay(MenuTransitionDelay);
            IsMenuOpen = false;
            MenuWidth = ZeroConstant;
        });

        ToggleSidePanelCommand = ReactiveCommand.Create(ToggleMenu);

        _timeTrackingControl = timeTrackingControl;
        _currentControl = authenticationControl;
        _tokenService = tokenService;
    }

    public Control CurrentControl
    {
        get => _currentControl;
        set => this.RaiseAndSetIfChanged(ref _currentControl, value);
    }

    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        set => this.RaiseAndSetIfChanged(ref _isAuthenticated, value);
    }

    public bool IsMenuOpen
    {
        get => _isMenuOpen;
        set => this.RaiseAndSetIfChanged(ref _isMenuOpen, value);
    }

    public int MenuWidth
    {
        get => _menuWidth;
        set => this.RaiseAndSetIfChanged(ref _menuWidth, value);
    }

    public ReactiveCommand<Unit, Unit> OnTrackingClickCommand { get; }
    public ReactiveCommand<Unit, Unit> OnExportClickCommand { get; }
    public ReactiveCommand<Unit, Unit> OnAnalysisClickCommand { get; }
    public ReactiveCommand<Unit, Unit> OnSettingsClickCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleSidePanelCommand { get; }
    public ReactiveCommand<Unit, Unit> OnDocumentationClickCommand { get; }

    private void ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        MenuWidth = IsMenuOpen ? MaxMenuWidth : ZeroConstant;
    }

    public void RegisterMessenger()
    {
        _tokenService.Authenticated += LoggedIn;
    }

    private Task LoggedIn(string token)
    {
        IsAuthenticated = !string.IsNullOrEmpty(token);

        if (!IsAuthenticated)
        {
            return Task.CompletedTask;
        }

        CurrentControl = _timeTrackingControl;
        return Task.CompletedTask;
    }

    public void UnregisterMessenger()
    {
        _tokenService.Authenticated -= LoggedIn;
    }
}