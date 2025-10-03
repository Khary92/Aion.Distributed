using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Communication.Mock.DataProvider;

public class MockSeeder
{
    private static readonly Random Rand = new();
    private readonly List<Guid> _notesGuids = [];
    private readonly List<Guid> _noteTypesGuids = [];
    private readonly Random _random = new();

    private readonly List<Guid> _sprintsGuids = [];
    private readonly List<Guid> _statisticsDataGuids = [];
    private readonly List<Guid> _tagsGuids = [];
    private readonly List<Guid> _ticketsGuids = [];
    private readonly List<Guid> _timeSlotsGuids = [];
    private readonly List<Guid> _workDayGuids = [];

    public List<TicketClientModel> Tickets { get; set; } = [];
    public List<StatisticsDataClientModel> StatisticsData { get; set; } = [];
    public List<TimeSlotClientModel> TimeSlots { get; set; } = [];
    public List<WorkDayClientModel> WorkDays { get; set; } = [];
    public List<NoteClientModel> Notes { get; set; } = [];
    public List<NoteTypeClientModel> NoteTypes { get; set; } = [];
    public List<SprintClientModel> Sprints { get; set; } = [];
    public List<TagClientModel> Tags { get; set; } = [];
    public List<DocumentationReplay> DocumentationReplays { get; set; } = [];

    public void Seed(MockSetup setup)
    {
        CreateGuids(setup);

        SeedTags(setup.AmountOfTags);
        SeedWorkDays(setup.WorkDayCount);
        SeedSprints(setup.SprintCount);
        SeedTickets(setup.TicketCount, setup.SprintCount);
        SeedTimeSlots(setup.TrackingSlotsCount, setup.WorkDayCount, setup.TicketCount, setup.AmountOfNotesPerTimeSlot);
        SeedStatisticsData(setup.TrackingSlotsCount, setup.AmountOfCheckedTagsPerTimeSlot);
        SeedNoteTypes(setup.NoteTypeCount);
        SeedNotes(setup.NoteCount, setup.NoteTypeCount, setup.TrackingSlotsCount, setup.TicketCount);
        SeedDocumentationReplays(setup.AmountOfReplayDocumentation);
    }

    private void SeedDocumentationReplays(int amountOfReplayDocumentation)
    {
        for (var i = 0; i < amountOfReplayDocumentation; i++)
            DocumentationReplays.Add(new DocumentationReplay(MockStringProvider.LoremIpsum()));
    }

    private void CreateGuids(MockSetup setup)
    {
        for (var i = 0; i < setup.AmountOfTags; i++) _tagsGuids.Add(Guid.NewGuid());
        for (var i = 0; i < setup.WorkDayCount; i++) _workDayGuids.Add(Guid.NewGuid());
        for (var i = 0; i < setup.SprintCount; i++) _sprintsGuids.Add(Guid.NewGuid());
        for (var i = 0; i < setup.TicketCount; i++) _ticketsGuids.Add(Guid.NewGuid());
        for (var i = 0; i < setup.TrackingSlotsCount; i++) _timeSlotsGuids.Add(Guid.NewGuid());
        for (var i = 0; i < setup.NoteTypeCount; i++) _noteTypesGuids.Add(Guid.NewGuid());
        for (var i = 0; i < setup.NoteCount; i++) _notesGuids.Add(Guid.NewGuid());
        for (var i = 0; i < setup.TrackingSlotsCount; i++) _statisticsDataGuids.Add(Guid.NewGuid());
    }

    private void SeedTags(int count)
    {
        for (var i = 0; i < count; i++)
            Tags.Add(new TagClientModel(_tagsGuids[i], MockStringProvider.RandomTag(), false));
    }

    private void SeedWorkDays(int count)
    {
        for (var i = 0; i < count; i++) WorkDays.Add(new WorkDayClientModel(_workDayGuids[i], DateTimeOffset.Now));
    }

    private void SeedSprints(int count)
    {
        for (var i = 0; i < count; i++)
            Sprints.Add(new SprintClientModel(
                _sprintsGuids[i],
                MockStringProvider.RandomSprintName(),
                true,
                DateTimeOffset.Now.AddDays(i * 14),
                DateTimeOffset.Now.AddDays((i + 1) * 14),
                []
            ));
    }

    private void SeedTickets(int ticketCount, int sprintCount)
    {
        for (var i = 0; i < ticketCount; i++)
            Tickets.Add(new TicketClientModel(
                _ticketsGuids[i],
                MockStringProvider.RandomTicketName(),
                "1234567890",
                MockStringProvider.LoremIpsum(),
                [_sprintsGuids[i % sprintCount]]
            ));

        foreach (var sprint in Sprints)
        {
            var ticketsForSprint = Tickets.Where(t => t.SprintIds.Contains(sprint.SprintId)).Select(t => t.TicketId)
                .ToList();
            sprint.TicketIds = ticketsForSprint;
        }
    }

    private void SeedTimeSlots(int slotCount, int workDayCount, int ticketCount, MockRanges notesPerSlot)
    {
        for (var i = 0; i < slotCount; i++)
            TimeSlots.Add(new TimeSlotClientModel(
                _timeSlotsGuids[i],
                WorkDays[i % workDayCount].WorkDayId,
                _ticketsGuids[i % ticketCount],
                DateTimeOffset.Now,
                DateTimeOffset.Now.AddHours(Rand.Next(1, 8)),
                [GetRandomizedGuids(_notesGuids, _random.Next(notesPerSlot.Min, notesPerSlot.Max))[0]],
                false
            ));
    }

    private void SeedStatisticsData(int slotCount, MockRanges checkedTagsPerSlot)
    {
        for (var i = 0; i < slotCount; i++)
        {
            var next = _random.Next(1, 4);

            StatisticsData.Add(new StatisticsDataClientModel(
                _statisticsDataGuids[i],
                _timeSlotsGuids[i],
                GetRandomizedGuids(_tagsGuids, _random.Next(checkedTagsPerSlot.Min, checkedTagsPerSlot.Max)),
                next == 1,
                next == 2,
                next == 3
            ));
        }
    }

    private void SeedNoteTypes(int count)
    {
        for (var i = 0; i < count; i++)
            NoteTypes.Add(new NoteTypeClientModel(_noteTypesGuids[i], MockStringProvider.RandomNoteType(),
                GetRandomHexColor()));
    }

    private void SeedNotes(int noteCount, int noteTypeCount, int slotCount, int ticketCount)
    {
        for (var i = 0; i < noteCount; i++)
        {
            var text = MockStringProvider.LoremIpsum(
                _random.Next(3, 8),
                _random.Next(8, 20),
                _random.Next(1, 3),
                _random.Next(2, 6),
                _random.Next(1, 3)
            );

            Notes.Add(new NoteClientModel(
                _notesGuids[i],
                text,
                NoteTypes[_random.Next(noteTypeCount)].NoteTypeId,
                TimeSlots[_random.Next(slotCount)].TimeSlotId,
                Tickets[_random.Next(ticketCount)].TicketId,
                DateTimeOffset.Now.AddMinutes(-_random.Next(0, 5000))
            ));
        }
    }

    private static string GetRandomHexColor()
    {
        var r = Rand.Next(0, 256);
        var g = Rand.Next(0, 256);
        var b = Rand.Next(0, 256);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private List<Guid> GetRandomizedGuids(List<Guid> guids, int amountRequired)
    {
        if (guids.Count < amountRequired)
            throw new ArgumentException("Not enough guids");

        if (guids.Count == amountRequired)
            return [..guids];

        var result = new List<Guid>();
        while (result.Count < amountRequired)
        {
            var potentialGuid = guids[_random.Next(guids.Count)];
            if (!result.Contains(potentialGuid))
                result.Add(potentialGuid);
        }

        return result;
    }
}