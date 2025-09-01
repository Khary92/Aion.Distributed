namespace Service.Monitoring.Shared.Enums;

public enum UseCaseMeta
{
    //Ticket
    CreateTicket,
    UpdateTicket,
    UpdateTicketDocumentation,


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
    ChangeTagSelection,
    AddNoteToTimeSlot,
    CreateTimeSlot,
    SetEndTime,
    SetStartTime
}