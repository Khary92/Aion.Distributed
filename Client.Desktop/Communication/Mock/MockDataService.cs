using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Mock.DataProvider;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Communication.Mock;

public class MockDataService
{
    private const int WorkDayCount = 1;
    private const int TrackingSlotsCount = 12;
    private const int SprintCount = 2;
    private const int TicketCount = 4;
    private const int TagCount = 7;
    private const int NoteCount = 30;
    private const int NoteTypeCount = 3;
    private const int AmountOfReplayDocumentation = 5;
    private const int MinimumAmountOfNotesPerTimeSlot = 3;
    private const int MaximumAmountOfNotesPerTimeSlot = 7;
    private const int MinimumAmountOfCheckedTagsPerTimeSlot = 2;
    private const int MaximumAmountOfCheckedTagsPerTimeSlot = 7;

    public MockDataService(IMockSeederFactory mockSeederFactory)
    {
        var mockSeeder = mockSeederFactory.Create(new MockSetup(WorkDayCount, TrackingSlotsCount, SprintCount,
            TicketCount, TagCount,
            NoteCount, NoteTypeCount, AmountOfReplayDocumentation,
            new MockRanges(MinimumAmountOfNotesPerTimeSlot, MaximumAmountOfNotesPerTimeSlot),
            new MockRanges(MinimumAmountOfCheckedTagsPerTimeSlot, MaximumAmountOfCheckedTagsPerTimeSlot)));

        StatisticsData = mockSeeder.StatisticsData;
        TimeSlots = mockSeeder.TimeSlots;
        Tickets = mockSeeder.Tickets;
        WorkDays = mockSeeder.WorkDays;
        Notes = mockSeeder.Notes;
        NoteTypes = mockSeeder.NoteTypes;
        Sprints = mockSeeder.Sprints;
        Tags = mockSeeder.Tags;
        DocumentationReplays = mockSeeder.DocumentationReplays;
    }

    public bool IsTimerSettingsExisting => true;

    public TimerSettingsClientModel TimerSettings => new(Guid.NewGuid(), 30, 30);

    public List<TicketClientModel> Tickets { get; set; }

    public List<StatisticsDataClientModel> StatisticsData { get; set; }

    public List<TimeSlotClientModel> TimeSlots { get; set; }

    public List<WorkDayClientModel> WorkDays { get; set; }

    public List<NoteClientModel> Notes { get; set; }

    public List<NoteTypeClientModel> NoteTypes { get; set; }

    public List<SprintClientModel> Sprints { get; set; }

    public List<TagClientModel> Tags { get; set; }

    public List<DocumentationReplay> DocumentationReplays { get; set; }

    public List<ClientGetTrackingControlResponse> GetInitialClientGetTrackingControlResponses()
    {
        var result = new List<ClientGetTrackingControlResponse>();
        foreach (var timeSlot in TimeSlots)
        {
            var statisticsData = StatisticsData.First(sd => sd.TimeSlotId == timeSlot.TimeSlotId);
            var ticket = Tickets.First(t => t.TicketId == timeSlot.SelectedTicketId);

            result.Add(new ClientGetTrackingControlResponse(statisticsData, ticket, timeSlot));
        }

        return result;
    }
}