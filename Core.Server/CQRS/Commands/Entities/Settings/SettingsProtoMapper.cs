using Proto.Command.Settings;

namespace Service.Server.CQRS.Commands.Entities.Settings;

public static class SettingsProtoMapper
{
    public static CreateSettingsCommand ToCommand(this CreateSettingsCommandProto proto) =>
        new(Guid.Parse(proto.SettingsId), proto.ExportPath, proto.IsAddNewTicketsToCurrentSprintActive);
    
    // TODO there are more distinct events now. Needs implementation
    //public static UpdateSettingsCommand ToCommand(this UpdateSettingsCommandProto proto) =>
}