using System.Threading.Tasks;
using Client.Desktop.Tracing.Tracing.Tracers;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Export;

public class ExportViewModel : ReactiveObject
{
    private readonly ITraceCollector _tracer;

    public ExportViewModel(ITraceCollector tracer, ExportModel exportModel)
    {
        _tracer = tracer;
        Model = exportModel;

        ExportCommand = ReactiveCommand.CreateFromTask(ExportFile);

        Model.SelectedWorkDays.CollectionChanged += async (_, _) =>
        {
            Model.MarkdownText = await Model.GetMarkdownTextAsync();
        };


        Model.InitializeAsync().ConfigureAwait(false);
        Model.RegisterMessenger();
    }

    public ExportModel Model { get; }

    public ReactiveCommand<Unit, Unit> ExportCommand { get; }

    private async Task ExportFile()
    {
        _tracer.Export.ToFile.StartUseCase(GetType());

        var success = await Model.ExportFileAsync();
        if (!success)
        {
            await ShowMessageBox("Config file is invalid!", "Please set an export path in settings.", Icon.Warning);
            return;
        }

        _tracer.Export.ToFile.ExportSuccessful(GetType());
        await ShowMessageBox("Great success!", "File exported successfully");
    }

    private static async Task ShowMessageBox(string title, string message, Icon icon = Icon.None)
    {
        await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = title,
            ContentMessage = message,
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = icon
        }).ShowAsync();
    }
}