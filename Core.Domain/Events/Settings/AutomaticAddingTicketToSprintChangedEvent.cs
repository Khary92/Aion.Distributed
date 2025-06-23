namespace Domain.Events.Settings;

public record AutomaticAddingTicketToSprintChangedEvent(
    Guid SettingsId,
    bool IsAddNewTicketsToCurrentSprintActive);