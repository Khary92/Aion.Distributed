namespace Core.Server.Communication.CQRS.Commands.Entities.Settings;

public record ChangeAutomaticTicketAddingToSprintCommand(Guid SettingsId, bool IsAddNewTicketsToCurrentSprintActive);