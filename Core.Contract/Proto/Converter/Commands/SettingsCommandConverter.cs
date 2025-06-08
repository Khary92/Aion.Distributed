using System;
using Contract.CQRS.Commands.Entities.Settings;
using Proto.Command.Settings;

namespace Contract.Converters
{
    public static class SettingsCommandConverter
    {
        public static CreateSettingsProtoCommand ToProto(this CreateSettingsCommand command)
            => new()
            {
                SettingsId = command.SettingsId.ToString(),
                ExportPath = command.ExportPath ?? string.Empty,
                IsAddNewTicketsToCurrentSprintActive = command.IsAddNewTicketsToCurrentSprintActive
            };

        public static UpdateSettingsProtoCommand ToProto(this UpdateSettingsCommand command)
            => new()
            {
                SettingsId = command.SettingsId.ToString(),
                ExportPath = command.ExportPath ?? string.Empty,
                IsAddNewTicketsToCurrentSprintActive = command.IsAddNewTicketsToCurrentSprintActive
            };

        public static CreateSettingsCommand ToDomain(this CreateSettingsProtoCommand proto)
            => new(Guid.Parse(proto.SettingsId), proto.ExportPath, proto.IsAddNewTicketsToCurrentSprintActive);

        public static UpdateSettingsCommand ToDomain(this UpdateSettingsProtoCommand proto)
            => new(Guid.Parse(proto.SettingsId), proto.ExportPath, proto.IsAddNewTicketsToCurrentSprintActive);
    }
}