using Proto.Notifications.Settings;
using ReactiveUI;

namespace Contract.DTO;

public class SettingsDto : ReactiveObject
{
    private readonly Guid _settingsId;
    private string _exportPath = string.Empty;
    private bool _isAddNewTicketsToCurrentSprintActive;
    
    private bool _previousIsAddNewTicketsToCurrentSprintActive;
    private string _previousExportPath;

    public SettingsDto(Guid settingsId, string exportPath, bool isAddNewTicketsToCurrentSprintActive)
    {
        SettingsId = settingsId;
        ExportPath = exportPath;
        IsAddNewTicketsToCurrentSprintActive = isAddNewTicketsToCurrentSprintActive;
        
        _previousExportPath = exportPath;
        _previousIsAddNewTicketsToCurrentSprintActive  = isAddNewTicketsToCurrentSprintActive;
    }

    public Guid SettingsId
    {
        get => _settingsId;
        private init => this.RaiseAndSetIfChanged(ref _settingsId, value);
    }

    public bool IsAddNewTicketsToCurrentSprintActive
    {
        get => _isAddNewTicketsToCurrentSprintActive;
        set => this.RaiseAndSetIfChanged(ref _isAddNewTicketsToCurrentSprintActive, value);
    }

    public string ExportPath
    {
        get => _exportPath;
        set => this.RaiseAndSetIfChanged(ref _exportPath, value);
    }

    public void Apply(AutomaticTicketAddingToSprintChangedNotification notification)
    {
        IsAddNewTicketsToCurrentSprintActive = notification.IsAddNewTicketsToCurrentSprintActive;
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

    public bool IsAddNewTicketsToCurrentSprintChanged()
    {
        if (IsAddNewTicketsToCurrentSprintActive == _previousIsAddNewTicketsToCurrentSprintActive) return false;
        
        _previousIsAddNewTicketsToCurrentSprintActive = IsAddNewTicketsToCurrentSprintActive;
        return true;
    }
}