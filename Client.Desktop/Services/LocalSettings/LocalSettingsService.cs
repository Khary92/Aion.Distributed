using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels.Local;
using Client.Desktop.FileSystem;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Newtonsoft.Json;

namespace Client.Desktop.Services.LocalSettings;

public class LocalSettingsService(
    IFileSystemReader fileSystemReader,
    IFileSystemWriter fileSystemWriter,
    IFileSystemWrapper fileSystemWrapper) : ILocalSettingsService, IInitializeAsync
{
    private const string SettingsFileName = "localSettings.json";

    private SettingsClientModel? ProjectionReferenceInstance { get; set; }

    public InitializationType Type => InitializationType.Service;

    public async Task InitializeAsync()
    {
        await LoadSettings();
    }

    public bool IsExportPathValid()
    {
        return fileSystemWrapper.IsDirectoryExisting(ProjectionReferenceInstance!.ExportPath);
    }

    public DateTimeOffset SelectedDate => ProjectionReferenceInstance?.SelectedDate ?? DateTimeOffset.Now;

    public bool IsSelectedDateCurrentDate()
    {
        return SelectedDate.Date == DateTimeOffset.Now.Date;
    }

    public string ExportPath => ProjectionReferenceInstance!.ExportPath;

    public async Task SetSelectedDate(DateTimeOffset date)
    {
        ProjectionReferenceInstance!.SelectedDate = date;
        await SaveSettings();
    }

    public async Task SetExportPath(string path)
    {
        ProjectionReferenceInstance!.ExportPath = path;
        await SaveSettings();
    }

    private async Task LoadSettings()
    {
        ProjectionReferenceInstance = fileSystemWrapper.IsFileExisting(SettingsFileName)
            ? (await fileSystemReader.GetObject<SettingsDto>(SettingsFileName)).ToClientModel()
            : new SettingsClientModel("not set");
    }

    private async Task SaveSettings()
    {
        if (ProjectionReferenceInstance == null) throw new NullReferenceException();

        var jsonString = JsonConvert.SerializeObject(ProjectionReferenceInstance.ToDto());
        await fileSystemWriter.Write(jsonString, SettingsFileName);
    }
}