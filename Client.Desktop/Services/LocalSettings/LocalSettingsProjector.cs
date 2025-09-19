using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels.Local;
using Client.Desktop.FileSystem;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;

namespace Client.Desktop.Services.LocalSettings;

public class LocalSettingsProjector(
    IFileSystemReader fileSystemReader,
    IFileSystemWriter fileSystemWriter,
    IFileSystemWrapper fileSystemWrapper,
    IMessenger messenger) : ILocalSettingsService, IInitializeAsync, IMessengerRegistration,
    IRecipient<ExportPathSetNotification>, IRecipient<WorkDaySelectedNotification>, IRecipient<SettingsClientModel>
{
    private const string SettingsFileName = "settings.json";

    private SettingsClientModel? ProjectionReferenceInstance { get; set; }

    public InitializationType Type => InitializationType.Service;

    public async Task InitializeAsync()
    {
        await PrepareProjection();
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

    public string GetExportPath()
    {
        return ProjectionReferenceInstance == null ? string.Empty : ProjectionReferenceInstance.ExportPath;
    }

    public void RegisterMessenger()
    {
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Receive(ExportPathSetNotification message)
    {
        ProjectionReferenceInstance!.ExportPath = message.ExportPath;
        _ = SaveSettings();
    }

    public void Receive(SettingsClientModel message)
    {
        ProjectionReferenceInstance = message;
        _ = SaveSettings();
    }

    public void Receive(WorkDaySelectedNotification message)
    {
        ProjectionReferenceInstance!.SelectedDate = message.Date;
        _ = SaveSettings();
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