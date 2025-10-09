using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Mock;

namespace Client.Desktop.Communication.Mock;

public class MockDataService(IMockSeederFactory mockSeederFactory, IMockSeedSetupService setupService)
    : IInitializeAsync
{
    public bool IsTimerSettingsExisting => true;

    public TimerSettingsClientModel TimerSettings => new(Guid.NewGuid(), 30, 30);

    public List<TicketClientModel> Tickets { get; set; } = [];

    public List<StatisticsDataClientModel> StatisticsData { get; set; } = [];

    public List<TimeSlotClientModel> TimeSlots { get; set; } = [];

    public List<WorkDayClientModel> WorkDays { get; set; } = [];

    public List<NoteClientModel> Notes { get; set; } = [];

    public List<NoteTypeClientModel> NoteTypes { get; set; } = [];

    public List<SprintClientModel> Sprints { get; set; } = [];

    public List<TagClientModel> Tags { get; set; } = [];

    public List<DocumentationReplay> DocumentationReplays { get; set; } = [];

    public List<ClientGetTrackingControlResponse> ClientGetTrackingControlResponse { get; set; } = [];

    public InitializationType Type => InitializationType.MockServices;

    public async Task InitializeAsync()
    {
        var mockSeeder = mockSeederFactory.Create(await setupService.ReadSetupFromFile());

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