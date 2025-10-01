using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.Export;

public class ExportViewModel : ReactiveObject
{
    public ExportViewModel(ExportModel exportModel)
    {
        Model = exportModel;

        ExportCommand = ReactiveCommand.CreateFromTask(ExportFile);

        Model.SelectedWorkDays.CollectionChanged += Model.RefreshMarkdownViewerHandler;
    }

    public ExportModel Model { get; }

    public ReactiveCommand<Unit, Unit>? ExportCommand { get; internal set; }

    private async Task ExportFile()
    {
        var success = await Model.ExportFileAsync();
        if (!success)
        {
            await ShowMessageBox("Config file is invalid!", "Please set an export path in settings.", Icon.Warning);
            return;
        }

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