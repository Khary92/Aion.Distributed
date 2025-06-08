using ReactiveUI;

namespace Contract.DTO;

public class SettingsDto : ReactiveObject
{
    private readonly Guid _settingsId;
    private string _exportPath = string.Empty;
    private bool _isAddNewTicketsToCurrentSprintActive;

    public SettingsDto(Guid settingsId, string exportPath, bool isAddNewTicketsToCurrentSprintActive)
    {
        SettingsId = settingsId;
        ExportPath = exportPath;
        IsAddNewTicketsToCurrentSprintActive = isAddNewTicketsToCurrentSprintActive;
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
}