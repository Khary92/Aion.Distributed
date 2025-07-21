using System;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.FileSystem;
using Client.Desktop.Services.Initializer;
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

    private SettingsDto? ProjectionReferenceInstance { get; set; }

    private async Task PrepareProjection()
    {
        var settings = fileSystemWrapper.IsFileExisting(SettingsFileName) ?
            await fileSystemReader.GetObject<SettingsDto>(SettingsFileName) :
            new SettingsDto("not set");
        
        messenger.Send(settings);
    }

    private async Task SaveSettings()
    {
        if (ProjectionReferenceInstance == null)
        {
            throw new NullReferenceException();
        }

        var jsonString = JsonConvert.SerializeObject(ProjectionReferenceInstance);
        await fileSystemWriter.Write(jsonString, SettingsFileName);
    }

    public async Task InitializeAsync()
    {
        messenger.Register<ExportPathSetNotification>(this, async void (_, m) =>
        {
            ProjectionReferenceInstance!.ExportPath = m.ExportPath;
            await SaveSettings();
        });
        
        messenger.Register<SettingsDto>(this, async void (_, m) =>
        {
            ProjectionReferenceInstance = m; 
            await SaveSettings();
        });
        
        await PrepareProjection();
    }

    public bool IsExportPathValid()
    {
        return fileSystemWrapper.IsFileExisting(ProjectionReferenceInstance!.ExportPath);
    }
}