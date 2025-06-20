using Proto.Command.Settings;
using Proto.DTO.Settings;
using Proto.Notifications.Settings;
using Service.Server.CQRS.Commands.Entities.Settings;

namespace Service.Server.Communication.Settings;

public static class SettingsProtoExtensions
{
    public static CreateSettingsCommand ToCommand(this CreateSettingsCommandProto proto) =>
        new(Guid.Parse(proto.SettingsId), proto.ExportPath, proto.IsAddNewTicketsToCurrentSprintActive);

    public static SettingsNotification ToNotification(this CreateSettingsCommandProto proto) =>
        new()
        {
            SettingsCreated = new SettingsCreatedNotification
            {
                SettingsId = proto.SettingsId,
                ExportPath = proto.ExportPath,
                IsAddNewTicketsToCurrentSprintActive = proto.IsAddNewTicketsToCurrentSprintActive
            }
        };

    public static ChangeExportPathCommand ToCommand(this ChangeExportPathCommandProto proto) =>
        new(Guid.Parse(proto.SettingsId), proto.ExportPath);

    public static SettingsNotification ToNotification(this ChangeExportPathCommandProto proto) =>
        new()
        {
            ExportPathChanged = new ExportPathChangedNotification
            {
                SettingsId = proto.SettingsId,
                ExportPath = proto.ExportPath,
            }
        };

    public static ChangeAutomaticTicketAddingToSprintCommand ToCommand(
        this ChangeAutomaticTicketAddingToSprintCommandProto proto) =>
        new(Guid.Parse(proto.SettingsId), proto.IsAddNewTicketsToCurrentSprintActive);

    public static SettingsNotification ToNotification(this ChangeAutomaticTicketAddingToSprintCommandProto proto) =>
        new()
        {
            AutomaticTicketAddingChanged = new AutomaticTicketAddingToSprintChangedNotification()
            {
                SettingsId = proto.SettingsId,
                IsAddNewTicketsToCurrentSprintActive = proto.IsAddNewTicketsToCurrentSprintActive
            }
        };

    public static SettingsProto ToProto(this Domain.Entities.Settings settings) =>
        new()
        {
            SettingsId = settings.SettingsId.ToString(),
            ExportPath = settings.ExportPath,
            IsAddNewTicketsToCurrentSprintActive = settings.IsAddNewTicketsToCurrentSprintActive
        };
}