using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels.Local;
using Client.Desktop.FileSystem;
using Client.Desktop.Lifecycle.Startup.Initialize;
using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;

namespace Client.Desktop.Services.LocalSettings;

public class LocalSettingsProjector(
    IFileSystemReader fileSystemReader,
    IFileSystemWriter fileSystemWriter,
    IFileSystemWrapper fileSystemWrapper,
    IMessenger messenger) : ILocalSettingsService, IInitializeAsync
{
    private const string SettingsFileName = "settings.json";

    private SettingsClientModel? ProjectionReferenceInstance { get; set; }

    public InitializationType Type => InitializationType.Service;

    public async Task InitializeAsync()
    {
        messenger.Register<SettingsClientModel>(this, async void (_, m) =>
        {
            ProjectionReferenceInstance = m;
            await SaveSettings();
        });

        messenger.Register<ExportPathSetNotification>(this, async void (_, m) =>
        {
            ProjectionReferenceInstance!.ExportPath = m.ExportPath;
            await SaveSettings();
        });

        messenger.Register<WorkDaySelectedNotification>(this, async void (_, m) =>
        {
            ProjectionReferenceInstance!.SelectedDate = m.Date;
            await SaveSettings();
        });

        await PrepareProjection();
    }

    public bool IsExportPathValid()
    {
        return fileSystemWrapper.IsDirectoryExisting(ProjectionReferenceInstance!.ExportPath);
    }

    public DateTimeOffset SelectedDate => ProjectionReferenceInstance!.SelectedDate;

    public bool IsSelectedDateCurrentDate()
    {
        return SelectedDate.Date == DateTimeOffset.Now.Date;
    }

    private async Task PrepareProjection()
    {
        var settings = fileSystemWrapper.IsFileExisting(SettingsFileName)
            ? (await fileSystemReader.GetObject<SettingsDto>(SettingsFileName)).ToClientModel()
            : new SettingsClientModel("not set");

        messenger.Send(settings);
    }

    private async Task SaveSettings()
    {
        if (ProjectionReferenceInstance == null) throw new NullReferenceException();

        var jsonString = JsonConvert.SerializeObject(ProjectionReferenceInstance.ToDto());
        await fileSystemWriter.Write(jsonString, SettingsFileName);
    }
}