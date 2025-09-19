using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Presentation.Views.Custom;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Factories;

public class TimeSlotViewModelFactory(IServiceProvider serviceProvider, IRequestSender requestSender)
    : ITimeSlotViewModelFactory
{
    public async Task<TimeSlotViewModel> Create(TicketClientModel ticket, StatisticsDataClientModel statisticsData,
        TimeSlotClientModel timeSlot)
    {
        var timeSlotViewModel = serviceProvider.GetRequiredService<TimeSlotViewModel>();

        var replayDecorator = new TicketReplayDecorator(requestSender, ticket)
        {
            DisplayedDocumentation = ticket.Documentation,
            IsReplayMode = false
        };

        timeSlotViewModel.Model.TicketReplayDecorator = replayDecorator;
        timeSlotViewModel.Model.TimeSlot = timeSlot;

        timeSlotViewModel.CreateSubViewModels(ticket.TicketId, timeSlot.TimeSlotId, statisticsData);

        await timeSlotViewModel.StatisticsViewModel.Initialize();
        timeSlotViewModel.StatisticsViewModel.RegisterMessenger();

        await timeSlotViewModel.NoteStreamViewModel.InitializeAsync();
        timeSlotViewModel.NoteStreamViewModel.RegisterMessenger();

        timeSlotViewModel.Model.RegisterMessenger();
        timeSlotViewModel.InitializeViewTimer(new ViewTimer());

        return timeSlotViewModel;
    }
}