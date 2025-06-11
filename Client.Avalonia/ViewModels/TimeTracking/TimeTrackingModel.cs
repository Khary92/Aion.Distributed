using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Notifications.Ticket;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Sprints;
using Contract.CQRS.Notifications.Entities.Tickets;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.ViewModels.TimeTracking;

public class TimeTrackingModel(IMessenger messenger, IMediator mediator) : ObservableObject
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
        FilteredTickets.Clear();
        var tickets = await mediator.Send(new GetTicketsForCurrentSprintRequest());
        FilteredTickets.AddRange(tickets);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTicketMessage>(this, async void (_, m) =>
        {
            var currentSprint = await mediator.Send(new GetActiveSprintRequest());

            AllTickets.Add(m.Ticket);
            if (currentSprint != null && currentSprint.TicketIds.Contains(m.Ticket.TicketId))
                FilteredTickets.Add(m.Ticket);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            var ticketDto = AllTickets.FirstOrDefault(tsv => tsv.TicketId == m.TicketId);

            if (ticketDto == null) return;

            ticketDto.Apply(m);
        });

        messenger.Register<TicketAddedToSprintNotification>(this, async void (_, _) => { await Initialize(); });
        messenger.Register<TicketAddedToActiveSprintNotification>(this, async void (_, _) => { await Initialize(); });
    }
}