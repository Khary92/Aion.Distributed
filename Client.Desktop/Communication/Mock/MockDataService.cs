using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Communication.Mock;

public class MockDataService
{
    private readonly List<Guid> _sprintsGuids = [Guid.NewGuid()];
    private readonly List<Guid> _ticketsGuids = [Guid.NewGuid()];
    private readonly List<Guid> _statisticsDataGuids = [Guid.NewGuid()];
    private readonly List<Guid> _timeSlotsGuids = [Guid.NewGuid()];
    private readonly List<Guid> _noteTypesGuids = [Guid.NewGuid()];
    private readonly List<Guid> _notesGuids = [Guid.NewGuid()];
    private readonly List<Guid> _tagsGuids = [Guid.NewGuid()];
    private readonly List<Guid> _workDayGuids = [Guid.NewGuid()];

    public bool IsTimerSettingsExisting => true;

    public TimerSettingsClientModel TimerSettings => new TimerSettingsClientModel(Guid.NewGuid(), 30, 30);

    public List<TicketClientModel> Tickets { get; set; } = [];

    public List<StatisticsDataClientModel> StatisticsData { get; set; } = [];

    public List<TimeSlotClientModel> TimeSlots { get; set; } = [];

    public List<WorkDayClientModel> WorkDays { get; set; } = [];

    public List<NoteClientModel> Notes { get; set; } = [];

    public List<NoteTypeClientModel> NoteTypes { get; set; } = [];

    public List<SprintClientModel> Sprints { get; set; } = [];

    public List<TagClientModel> Tags { get; set; } = [];

    public static List<DocumentationReplay> DocumentationReplays =>
    [
        new("Documentation 1"),
        new("Documentation 2"),
        new("Documentation 3"),
        new("Documentation 4"),
        new("Documentation 5"),
        new("Documentation 6"),
        new("Documentation 7"),
        new("Documentation 8"),
        new("Documentation 9"),
        new("Documentation 10"),
        new("Documentation 11")
    ];

    public MockDataService()
    {
        SeedWorkDays();
        SeedNoteTypes();
        SeedSprints();
        SeedTickets();
        SeedTimeSlots();
        SeedNotes();
        SeedTags();
        SeedStatisticsData();
    }

    private void SeedWorkDays()
    {
        WorkDays.Add(new WorkDayClientModel(_workDayGuids.First(), DateTimeOffset.Now));
    }

    private void SeedNoteTypes()
    {
        NoteTypes.Add(new NoteTypeClientModel(_noteTypesGuids.First(), "Note Type 1", "#000000"));
    }

    private void SeedNotes()
    {
        Notes.Add(new NoteClientModel(
            _notesGuids.First(),
            "Note 1",
            NoteTypes.First().NoteTypeId,
            TimeSlots.First().TimeSlotId,
            Tickets.First().TicketId,
            DateTimeOffset.Now
        ));
    }

    private void SeedTickets()
    {
        Tickets.Add(new TicketClientModel(
            _ticketsGuids.First(),
            "Ticket 1",
            "1234567890",
            "Documentation 1",
            [_sprintsGuids.First()]
        ));
    }

    private void SeedTimeSlots()
    {
        TimeSlots.Add(new TimeSlotClientModel(
            _timeSlotsGuids.First(),
            WorkDays.First().WorkDayId,
            Tickets.First().TicketId,
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddHours(3),
            [_notesGuids.First()],
            false
        ));
    }

    private void SeedTags()
    {
        Tags.Add(new TagClientModel(_tagsGuids.First(), "Tag 1", false));
    }

    private void SeedStatisticsData()
    {
        StatisticsData.Add(new StatisticsDataClientModel(
            _statisticsDataGuids.First(),
            TimeSlots.First().TimeSlotId,
            [Tags.First().TagId],
            true,
            false,
            false
        ));
    }

    private void SeedSprints()
    {
        Sprints.Add(new SprintClientModel(
            _sprintsGuids.First(),
            "A sprint",
            true,
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddDays(14),
            [_ticketsGuids.First()]
        ));
    }

    public List<ClientGetTrackingControlResponse> GetInitialClientGetTrackingControlResponses()
    {
        return [new(StatisticsData.First(), Tickets.First(), TimeSlots.First())];
    }
}