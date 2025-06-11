using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Notifications.Ticket;
using Client.Avalonia.Communication.Requests;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using DynamicData;
using Proto.Command.Sprints;
using Proto.Command.Tickets;
using Proto.Notifications.Ticket;
using ReactiveUI;

namespace Client.Avalonia.Models.Data;

public class TicketsDataModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
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
            Tickets.AddRange(await requestSender.GetAllTickets());
            return;
        }

        Tickets.AddRange(await requestSender.GetTicketsForCurrentSprint());
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
            var parsedId = Guid.Parse(m.TicketId);
            tracer.Ticket.Update.NotificationReceived(GetType(), parsedId, m);

            var ticket = Tickets.FirstOrDefault(t => t.TicketId == parsedId);

            if (ticket is null)
            {
                tracer.Ticket.Update.NoAggregateFound(GetType(), parsedId);
                return;
            }

            ticket.Apply(m);
            tracer.Ticket.Update.ChangesApplied(GetType(), parsedId);
        });
    }

    public async Task AddTicketToCurrentSprint(TicketDto ticketDto)
    {
        var addTicketToActiveSprintCommand = new AddTicketToActiveSprintCommand
            { TicketId = ticketDto.TicketId.ToString() };

        await commandSender.Send(addTicketToActiveSprintCommand);

        tracer.Ticket.AddTicketToSprint.CommandSent(GetType(), ticketDto.TicketId,
            addTicketToActiveSprintCommand);
    }

    public async Task UpdateTicket(TicketDto selectedTicket)
    {
        var updateTicketDataCommand = new UpdateTicketDataCommand
        {
            TicketId = selectedTicket.TicketId.ToString(),
            Name = selectedTicket.Name,
            BookingNumber = selectedTicket.BookingNumber,
        };
        updateTicketDataCommand.SprintIds.AddRange(selectedTicket.SprintIds.Select(id => id.ToString()));

        await commandSender.Send(updateTicketDataCommand);

        tracer.Ticket.Update.CommandSent(GetType(), selectedTicket.TicketId, updateTicketDataCommand);
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