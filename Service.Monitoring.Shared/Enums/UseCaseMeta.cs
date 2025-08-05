namespace Service.Monitoring.Shared.Enums;

public enum UseCaseMeta
{
    //Ticket
    CreateTicket,
    UpdateTicket,
    UpdateTicketDocumentation,

    
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
    CommandSent,
    CreateStatisticsData,
    ChangeProductivity,
    ChangeTagSelection
}