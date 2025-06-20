using Proto.Command.Settings;
using Proto.DTO.Settings;
using Proto.Notifications.Settings;
using Service.Server.Communication.CQRS.Commands.Entities.Settings;

namespace Service.Server.Communication.Services.Settings;

public static class SettingsProtoExtensions
{
    public static CreateSettingsCommand ToCommand(this CreateSettingsCommandProto proto) =>
        new(Guid.Parse(proto.SettingsId), proto.ExportPath, proto.IsAddNewTicketsToCurrentSprintActive);

    public static SettingsNotification ToNotification(this CreateSettingsCommand proto) =>
        new()
        {
            SettingsCreated = new SettingsCreatedNotification
            {
                SettingsId = proto.SettingsId.ToString(),
                ExportPath = proto.ExportPath,
                IsAddNewTicketsToCurrentSprintActive = proto.IsAddNewTicketsToCurrentSprintActive
            }
        };

    public static ChangeExportPathCommand ToCommand(this ChangeExportPathCommandProto proto) =>
        new(Guid.Parse(proto.SettingsId), proto.ExportPath);

    public static SettingsNotification ToNotification(this ChangeExportPathCommand proto) =>
        new()
        {
            ExportPathChanged = new ExportPathChangedNotification
            {
                SettingsId = proto.SettingsId.ToString(),
                ExportPath = proto.ExportPath,
            }
        };

    public static ChangeAutomaticTicketAddingToSprintCommand ToCommand(
        this ChangeAutomaticTicketAddingToSprintCommandProto proto) =>
        new(Guid.Parse(proto.SettingsId), proto.IsAddNewTicketsToCurrentSprintActive);

    public static SettingsNotification ToNotification(this ChangeAutomaticTicketAddingToSprintCommand proto) =>
        new()
        {
            AutomaticTicketAddingChanged = new AutomaticTicketAddingToSprintChangedNotification()
            {
                SettingsId = proto.SettingsId.ToString(),
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