using System.Threading.Tasks;
using Avalonia.Controls;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Presentation.Views.Analysis;
using Client.Desktop.Presentation.Views.Documentation;
using Client.Desktop.Presentation.Views.Export;
using Client.Desktop.Presentation.Views.Setting;
using Client.Desktop.Presentation.Views.Tracking;
using ReactiveUI;
using Unit = System.Reactive.Unit;


namespace Client.Desktop.Presentation.Models.Main;

public class ContentWrapperViewModel : ReactiveObject
{
    private const int ZeroConstant = 0;
    private const int MenuTransitionDelay = 250;
    private const int MaxMenuWidth = 200;

    private Control _currentControl;

    private bool _isMenuOpen;

    private int _menuWidth;

    public ContentWrapperViewModel(SettingsCompositeControl settingsCompositeControl,
        TimeTrackingControl timeTrackingControl,
        ExportControl exportControl, AnalysisControlWrapper analysisControlWrapper,
        TimeTrackingViewModel timeTrackingViewModel, DocumentationControl documentationControl)
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

        _currentControl = timeTrackingControl;
    }

    public Control CurrentControl
    {
        get => _currentControl;
        set => this.RaiseAndSetIfChanged(ref _currentControl, value);
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
}