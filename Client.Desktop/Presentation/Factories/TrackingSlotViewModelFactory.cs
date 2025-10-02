using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Replay;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Presentation.Views.Custom;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Factories;

public class TrackingSlotViewModelFactory(
    IServiceProvider serviceProvider,
    IDocumentationSynchronizer documentationSynchronizer)
    : ITrackingSlotViewModelFactory
{
    public async Task<TrackingSlotViewModel> Create(TicketClientModel ticket, StatisticsDataClientModel statisticsData,
        TimeSlotClientModel timeSlot)
    {
        var trackingSlotViewModel = serviceProvider.GetRequiredService<TrackingSlotViewModel>();
        var ticketReplayProvider = serviceProvider.GetRequiredService<ITicketReplayProvider>();

        ticket.TicketReplayProvider = ticketReplayProvider;
        ticket.DocumentationSynchronizer = documentationSynchronizer;
        trackingSlotViewModel.Model.Ticket = ticket;
        trackingSlotViewModel.Model.TimeSlot = timeSlot;

        trackingSlotViewModel.Model.RegisterMessenger();

        await trackingSlotViewModel.CreateSubViewModels(ticket.TicketId, timeSlot.TimeSlotId, statisticsData);

        await trackingSlotViewModel.StatisticsViewModel.InitializeAsync();
        trackingSlotViewModel.StatisticsViewModel.RegisterMessenger();

        trackingSlotViewModel.InitializeViewTimer(new ViewTimer());

        return trackingSlotViewModel;
    }
}