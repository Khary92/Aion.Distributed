namespace Service.Admin.Web.DTO;

public class TimerSettingsDto(Guid timerSettingsId, int documentationSaveInterval, int snapshotSaveInterval)
{
    public Guid TimerSettingsId { get; private set; } = timerSettingsId;
    public int DocumentationSaveInterval { get; set; } = documentationSaveInterval;
    public int SnapshotSaveInterval { get; set; } = snapshotSaveInterval;
}