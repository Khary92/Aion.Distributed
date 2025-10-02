using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
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
        var timeSlotViewModel = serviceProvider.GetRequiredService<TrackingSlotViewModel>();

        ticket.DocumentationSynchronizer = documentationSynchronizer;
        timeSlotViewModel.Model.Ticket = ticket;
        timeSlotViewModel.Model.TimeSlot = timeSlot;
        timeSlotViewModel.Model.RegisterMessenger();

        await timeSlotViewModel.CreateSubViewModels(ticket.TicketId, timeSlot.TimeSlotId, statisticsData);

        await timeSlotViewModel.StatisticsViewModel.InitializeAsync();
        timeSlotViewModel.StatisticsViewModel.RegisterMessenger();
        
        timeSlotViewModel.InitializeViewTimer(new ViewTimer());

        return timeSlotViewModel;
    }
}