using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using Client.Desktop.Models.TimeTracking;
using Client.Desktop.Replays;
using Client.Desktop.Views.Custom;
using Microsoft.Extensions.DependencyInjection;
using Proto.Requests.StatisticsData;
using Proto.Requests.Tickets;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Factories;

public class TimeSlotViewModelFactory(IServiceProvider serviceProvider, IRequestSender requestSender)
    : ITimeSlotViewModelFactory
{ 
    public async Task<TimeSlotViewModel> Create(TicketDto ticket, StatisticsDataDto statisticsData, TimeSlotDto timeSlot)
    {
        var timeSlotViewModel = serviceProvider.GetRequiredService<TimeSlotViewModel>();
        
        var replayDecorator = new TicketReplayDecorator(requestSender, ticket)
        {
            DisplayedDocumentation = ticket.Documentation,
            IsReplayMode = false
        };

        timeSlotViewModel.Model.TicketReplayDecorator = replayDecorator;
        timeSlotViewModel.Model.TimeSlot = timeSlot;
        timeSlotViewModel.StatisticsViewModel.StatisticsData = statisticsData;

        await timeSlotViewModel.StatisticsViewModel.Initialize();
        timeSlotViewModel.StatisticsViewModel.RegisterMessenger();

        timeSlotViewModel.NoteStreamViewModel.TimeSlotId = timeSlot.TimeSlotId;
        await timeSlotViewModel.NoteStreamViewModel.InitializeAsync();
        timeSlotViewModel.NoteStreamViewModel.RegisterMessenger();

        timeSlotViewModel.InitializeViewTimer(new ViewTimer());
        return timeSlotViewModel;
    }
}