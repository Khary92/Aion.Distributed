using System.Text.Json;
using Domain.Events.TimerSettings;

namespace Domain.Entities;

public class TimerSettings
{
    public Guid TimerSettingsId { get; set; }
    public int DocumentationSaveInterval { get; set; }
    public int SnapshotSaveInterval { get; set; }

    public static TimerSettings Rehydrate(IEnumerable<TimerSettingsEvent> events)
    {
        var timerSettings = new TimerSettings();

        foreach (var evt in events) timerSettings.Apply(evt);

        return timerSettings;
    }

    private void Apply(TimerSettingsEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(TimerSettingsCreatedEvent):
                var created = JsonSerializer.Deserialize<TimerSettingsCreatedEvent>(evt.EventPayload);
                TimerSettingsId = created!.TimerSettingsId;
                DocumentationSaveInterval = created.DocumentationSaveInterval;
                SnapshotSaveInterval = created.SnapshotSaveInterval;
                break;

            case nameof(SnapshotIntervalChangedEvent):
                var snapshotInterval = JsonSerializer.Deserialize<SnapshotIntervalChangedEvent>(evt.EventPayload);
                SnapshotSaveInterval = snapshotInterval!.SnapshotSaveInterval;
                break;

            case nameof(DocuIntervalChangedEvent):
                var documentationInterval = JsonSerializer.Deserialize<DocuIntervalChangedEvent>(evt.EventPayload);
                DocumentationSaveInterval = documentationInterval!.DocumentationSaveInterval;
                break;
        }
    }
}