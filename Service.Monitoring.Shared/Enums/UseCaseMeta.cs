namespace Service.Monitoring.Shared.Enums;

public enum UseCaseMeta
{
    //NoteType
    ChangeNoteTypeColor,
    ChangeNoteTypeName,
    CreateNoteType,
    //Note
    CreateNote,
    UpdateNote,
    //Sprint
    CreateSprint,
    UpdateSprint,
    ChangeSprintActiveStatus,
    AddTicketToCurrentSprint,
    //StatisticsData
    CreateStatisticsData,
    ChangeProductivity,
    ChangeTagSelection,
    //Tag
    CreateTag,
    UpdateTag,
    //Ticket
    CreateTicket,
    UpdateTicket,
    UpdateTicketDocumentation,
    //TimerSettings
    ChangeDocuTimerSaveInterval,
    ChangeSnapshotSaveInterval,
    //TimeSlot
    CreateTimeSlot,
    AddNoteToTimeSlot,
    SetStartTime,
    SetEndTime,
    //WorkDay
    CreateWorkDay
}