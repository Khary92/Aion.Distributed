using System.Collections.Generic;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Communication.Mock.DataProvider;

public interface IMockSeeder
{
    List<TicketClientModel> Tickets { get; set; }
    List<StatisticsDataClientModel> StatisticsData { get; set; }
    List<TimeSlotClientModel> TimeSlots { get; set; }
    List<WorkDayClientModel> WorkDays { get; set; }
    List<NoteClientModel> Notes { get; set; }
    List<NoteTypeClientModel> NoteTypes { get; set; }
    List<SprintClientModel> Sprints { get; set; }
    List<TagClientModel> Tags { get; set; }
    List<DocumentationReplay> DocumentationReplays { get; set; }
    void Seed(MockSetup setup);
}