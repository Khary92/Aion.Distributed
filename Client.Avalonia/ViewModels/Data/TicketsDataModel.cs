using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Notifications.Ticket;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Commands.Entities.Sprints;
using Contract.CQRS.Notifications.Entities.Tickets;
using Contract.CQRS.Requests.Tickets;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using DynamicData;
using MediatR;
using Proto.Command.Tickets;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Data;

public class TicketsDataModel(
    TicketCommandSender commandSender,
    IMediator mediator,
    IMessenger messenger,
    ITracingCollectorProvider tracer)
    : ReactiveObject
{
    public ObservableCollection<TicketDto> Tickets { get; } = [];

    public async Task InitializeAsync(bool isShowAllTicketsActive)
    {
        Tickets.Clear();

        if (isShowAllTicketsActive)
        {
            Tickets.AddRange(await mediator.Send(new GetAllTicketsRequest()));
            return;
        }

        Tickets.AddRange(await mediator.Send(new GetTicketsForCurrentSprintRequest()));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTicketMessage>(this, (_, m) =>
        {
            tracer.Ticket.Create.AggregateReceived(GetType(), m.Ticket.TicketId, m.Ticket.AsTraceAttributes());
            Tickets.Add(m.Ticket);
            tracer.Ticket.Create.AggregateAdded(GetType(), m.Ticket.TicketId);
        });

        messenger.Register<TicketDataUpdatedNotification>(this, (_, m) =>
        {
            tracer.Ticket.Update.NotificationReceived(GetType(), m.TicketId, m);

            var ticket = Tickets.FirstOrDefault(t => t.TicketId == m.TicketId);

            if (ticket is null)
            {
                tracer.Ticket.Update.NoAggregateFound(GetType(), m.TicketId);
                return;
            }

            ticket.Apply(m);
            tracer.Ticket.Update.ChangesApplied(GetType(), m.TicketId);
        });
    }

    public async Task AddTicketToCurrentSprint(TicketDto ticketDto)
    {
        var addTicketToActiveSprintCommand = new AddTicketToActiveSprintCommand(ticketDto.TicketId);
        await mediator.Send(addTicketToActiveSprintCommand);

        tracer.Ticket.AddTicketToSprint.CommandSent(GetType(), addTicketToActiveSprintCommand.TicketId,
            addTicketToActiveSprintCommand);
    }

    public async Task UpdateTicket(TicketDto selectedTicket)
    {
        var updateTicketDataCommand = new UpdateTicketDataCommand(selectedTicket.TicketId, selectedTicket.Name,
            selectedTicket.BookingNumber, selectedTicket.SprintIds);
        await mediator.Send(updateTicketDataCommand);

        tracer.Ticket.Update.CommandSent(GetType(), updateTicketDataCommand.TicketId, updateTicketDataCommand);
    }

    public async Task CreateTicket(TicketDto createTicketDto)
    {
        var createTicketCommand = new CreateTicketCommand
        {
            TicketId = createTicketDto.TicketId.ToString(),
            Name = createTicketDto.Name,
            BookingNumber = createTicketDto.BookingNumber,
            SprintIds = { createTicketDto.SprintIds.Select(guid => guid.ToString()) }
        };

        tracer.Ticket.Create.CommandSent(GetType(), createTicketDto.TicketId, createTicketCommand);

        await commandSender.Send(createTicketCommand);
    }
}