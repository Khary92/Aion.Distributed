using Contract.CQRS.Commands.Entities.TimerSettings;
using Proto.Command.TimerSettings;

namespace Contract.Proto.Converter.Commands
{
    public static class TimerSettingsCommandConverter
    {
        public static ChangeDocuTimerSaveIntervalProtoCommand ToProto(this ChangeDocuTimerSaveIntervalCommand command)
            => new()
            {
                TimerSettingsId = command.TimerSettingsId.ToString(),
                DocuTimerSaveInterval = command.DocuTimerSaveInterval
            };

        public static ChangeSnapshotSaveIntervalProtoCommand ToProto(this ChangeSnapshotSaveIntervalCommand command)
            => new()
            {
                TimerSettingsId = command.TimerSettingsId.ToString(),
                SnapshotSaveInterval = command.SnapshotSaveInterval
            };

        public static CreateTimerSettingsProtoCommand ToProto(this CreateTimerSettingsCommand command)
            => new()
            {
                TimerSettingsId = command.TimerSettingsId.ToString(),
                DocumentationSaveInterval = command.DocumentationSaveInterval,
                SnapshotSaveInterval = command.SnapshotSaveInterval
            };
    }
}