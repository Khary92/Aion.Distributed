using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.FileSystem;
using Newtonsoft.Json;

namespace Client.Desktop.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string SettingsFileName = "settings.json";

    private readonly IFileSystemReader _fileSystemReader;
    private readonly IFileSystemWriter _fileSystemWriter;
    private readonly IFileSystemWrapper _fileSystemWrapper;
    private readonly SettingsDto _localSettings;
    
    public SettingsDto LocalSettings => _localSettings;
    public string ExportPath => _localSettings.ExportPath;
    public LocalSettingsService(IFileSystemReader fileSystemReader, IFileSystemWriter fileSystemWriter,
        IFileSystemWrapper fileSystemWrapper)
    {
        _fileSystemReader = fileSystemReader;
        _fileSystemWriter = fileSystemWriter;
        _fileSystemWrapper = fileSystemWrapper;

        //TODO Implement Initialization Pattern -> Interface with central Initializer
        _localSettings = LoadSettings().Result;
    }

    private async Task<SettingsDto> LoadSettings()
    {
        if (_fileSystemWrapper.IsFileExisting(SettingsFileName))
        {
            return await _fileSystemReader.GetObject<SettingsDto>(SettingsFileName);
        }

        var newSettings = new SettingsDto("not set");
        await SaveSettings(newSettings);
        return newSettings;
    }
    
    public async Task SetExportPath(string exportPath)
    {
        LocalSettings.ExportPath = exportPath;
        await LocalSettings.Changed;
        await SaveSettings(LocalSettings);
    }
    
    public bool IsExportPathValid()
    {
        return _fileSystemWrapper.IsFileExisting(SettingsFileName);
    }

    private async Task SaveSettings(SettingsDto settingsDto)
    {
        var jsonString = JsonConvert.SerializeObject(settingsDto);
        await _fileSystemWriter.Write(jsonString, SettingsFileName);
    }
    
    public DateTimeOffset SelectedDate { get; set; } = DateTimeOffset.Now;

    public bool IsSelectedDateCurrentDate()
    {
        return SelectedDate.Date == DateTimeOffset.Now.Date;
    }
}