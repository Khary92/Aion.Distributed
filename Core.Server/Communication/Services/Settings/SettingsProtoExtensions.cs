using Core.Server.Communication.CQRS.Commands.Entities.Settings;
using Proto.Command.Settings;
using Proto.DTO.Settings;
using Proto.Notifications.Settings;

namespace Core.Server.Communication.Services.Settings;

public static class SettingsProtoExtensions
{
    public static CreateSettingsCommand ToCommand(this CreateSettingsCommandProto proto)
    {
        return new CreateSettingsCommand(Guid.Parse(proto.SettingsId), proto.ExportPath,
            proto.IsAddNewTicketsToCurrentSprintActive);
    }

    public static SettingsNotification ToNotification(this CreateSettingsCommand proto)
    {
        return new SettingsNotification
        {
            SettingsCreated = new SettingsCreatedNotification
            {
                SettingsId = proto.SettingsId.ToString(),
                ExportPath = proto.ExportPath,
                IsAddNewTicketsToCurrentSprintActive = proto.IsAddNewTicketsToCurrentSprintActive
            }
        };
    }

    public static ChangeExportPathCommand ToCommand(this ChangeExportPathCommandProto proto)
    {
        return new ChangeExportPathCommand(Guid.Parse(proto.SettingsId), proto.ExportPath);
    }

    public static SettingsNotification ToNotification(this ChangeExportPathCommand proto)
    {
        return new SettingsNotification
        {
            ExportPathChanged = new ExportPathChangedNotification
            {
                SettingsId = proto.SettingsId.ToString(),
                ExportPath = proto.ExportPath
            }
        };
    }

    public static ChangeAutomaticTicketAddingToSprintCommand ToCommand(
        this ChangeAutomaticTicketAddingToSprintCommandProto proto)
    {
        return new ChangeAutomaticTicketAddingToSprintCommand(Guid.Parse(proto.SettingsId),
            proto.IsAddNewTicketsToCurrentSprintActive);
    }

    public static SettingsNotification ToNotification(this ChangeAutomaticTicketAddingToSprintCommand proto)
    {
        return new SettingsNotification
        {
            AutomaticTicketAddingChanged = new AutomaticTicketAddingToSprintChangedNotification
            {
                SettingsId = proto.SettingsId.ToString(),
                IsAddNewTicketsToCurrentSprintActive = proto.IsAddNewTicketsToCurrentSprintActive
            }
        };
    }

    public static SettingsProto ToProto(this Domain.Entities.Settings settings)
    {
        return new SettingsProto
        {
            SettingsId = settings.SettingsId.ToString(),
            ExportPath = settings.ExportPath,
            IsAddNewTicketsToCurrentSprintActive = settings.IsAddNewTicketsToCurrentSprintActive
        };
    }
}