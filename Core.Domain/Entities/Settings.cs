using System.Text.Json;
using Domain.Events.Settings;

namespace Domain.Entities;

public class Settings
{
    public Guid SettingsId { get; set; }
    public string ExportPath { get; set; } = string.Empty;
    public bool IsAddNewTicketsToCurrentSprintActive { get; set; }

    public static Settings Rehydrate(IEnumerable<SettingsEvent> events)
    {
        var settings = new Settings();

        foreach (var evt in events) settings.Apply(evt);

        return settings;
    }

    private void Apply(SettingsEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(SettingsCreatedEvent):
                var created = JsonSerializer.Deserialize<SettingsCreatedEvent>(evt.EventPayload);
                SettingsId = created!.SettingsId;
                ExportPath = created.ExportPath;
                IsAddNewTicketsToCurrentSprintActive = created.IsAddNewTicketsToCurrentSprintActive;
                break;

            case nameof(ExportPathChangedEvent):
                var exportPath = JsonSerializer.Deserialize<ExportPathChangedEvent>(evt.EventPayload);
                ExportPath = exportPath!.ExportPath;
                break;

            case nameof(AutomaticAddingTicketToSprintChangedEvent):
                var isAddNewTicketsToCurrentSprintActive =
                    JsonSerializer.Deserialize<AutomaticAddingTicketToSprintChangedEvent>(evt.EventPayload);
                IsAddNewTicketsToCurrentSprintActive =
                    isAddNewTicketsToCurrentSprintActive!.IsAddNewTicketsToCurrentSprintActive;
                break;
        }
    }
}