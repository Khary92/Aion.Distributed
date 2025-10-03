namespace Client.Desktop.Communication.Mock.DataProvider;

public record MockSetup(
    int WorkDayCount,
    int TrackingSlotsCount,
    int SprintCount,
    int TicketCount,
    int AmountOfTags,
    int NoteCount,
    int NoteTypeCount,
    int AmountOfReplayDocumentation,
    MockRanges AmountOfNotesPerTimeSlot,
    MockRanges AmountOfCheckedTagsPerTimeSlot);