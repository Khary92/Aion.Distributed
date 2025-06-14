using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Notifications.Ticket;
using Client.Desktop.Communication.Requests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using DynamicData;
using Proto.Notifications.Sprint;
using Proto.Notifications.Ticket;
using Proto.Requests.Sprints;
using Proto.Requests.Tickets;

namespace Client.Desktop.Models.TimeTracking;

public class TimeTrackingModel(IRequestSender requestSender, IMessenger messenger) : ObservableObject
{
    private ObservableCollection<TicketDto> _filteredTickets = [];

    public ObservableCollection<TicketDto> FilteredTickets
    {
        get => _filteredTickets;
        set => SetProperty(ref _filteredTickets, value);
    }

    private ObservableCollection<TicketDto> AllTickets { get; } = [];

    public async Task Initialize()
    {
        var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

        if (currentSprint == null) return;
        
        FilteredTickets.Clear();
        var tickets = await requestSender.Send(new GetTicketsForCurrentSprintRequestProto());
        FilteredTickets.AddRange(tickets);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTicketMessage>(this, async void (_, m) =>
        {
            var currentSprint = await requestSender.Send(new GetActiveSprintRequestProto());

            AllTickets.Add(m.Ticket);
            if (currentSprint.TicketIds.Contains(m.Ticket.TicketId))
                FilteredTickets.Add(m.Ticket);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            var ticketDto = AllTickets.FirstOrDefault(tsv => tsv.TicketId == Guid.Parse(m.TicketId));

            if (ticketDto == null) return;

            ticketDto.Apply(m);
        });

        messenger.Register<TicketAddedToSprintNotification>(this, async void (_, _) => { await Initialize(); });
        messenger.Register<TicketAddedToActiveSprintNotification>(this, async void (_, _) => { await Initialize(); });
    }
}