namespace Service.Server.CQRS.Commands.Entities.Settings;

public record ChangeAutomaticTicketAddingToSprintCommand(Guid SettingsId, bool IsAddNewTicketsToCurrentSprintActive);