﻿namespace Core.Server.Communication.Records.Commands.Entities.Settings;

public record ChangeAutomaticTicketAddingToSprintCommand(Guid SettingsId, bool IsAddNewTicketsToCurrentSprintActive);