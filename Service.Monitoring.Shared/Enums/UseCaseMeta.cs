namespace Service.Monitoring.Shared.Enums;

public enum UseCaseMeta
{
    //Ticket
    CreateTicket,
    UpdateTicket,
    UpdateTicketDocumentation,

    //AI Settings
    ChangeLanguageModel,
    ChangePrompt,
    ExportData,
    CreateNote,
    UpdateNote,
    ChangeNoteTypeColor,
    ChangeNoteTypeName,
    CreateNoteType,
    CreateTag,
    UpdateTag,
    CreateSprint,
    ChangeSprintActiveStatus,
    TicketAddedToSprint,
    UpdateSprint,
    AddTicketToCurrentSprint,
    ChangeDocuTimerSaveInterval,
    ChangeSnapshotSaveInterval,
    CreateTimerSettings,
    CreateWorkDay,
    CommandSent
}