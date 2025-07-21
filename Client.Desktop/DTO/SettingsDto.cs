using System;
using Proto.Notifications.Settings;
using ReactiveUI;

namespace Client.Desktop.DTO;

public class SettingsDto : ReactiveObject
{
    private string _exportPath = string.Empty;
    private string _previousExportPath;
    
    public SettingsDto(string exportPath)
    {
        ExportPath = exportPath;
        _previousExportPath = exportPath;
    }
    
    public string ExportPath
    {
        get => _exportPath;
        set => this.RaiseAndSetIfChanged(ref _exportPath, value);
    }
    
    public void Apply(ExportPathChangedNotification notification)
    {
        ExportPath = notification.ExportPath;
    }

    public bool IsExportPathChanged()
    {
        if (ExportPath == _previousExportPath) return false;

        _previousExportPath = ExportPath;
        return true;
    }
}