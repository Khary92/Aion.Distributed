using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Mock;
using Client.Desktop.Communication.Notifications.Ticket.Receiver;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Proto;
using Proto.Command.Tickets;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;
using ReactiveUI;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Requests.Tickets;
using static System.Guid;
using Dispatcher = Avalonia.Threading.Dispatcher;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerTicketDataModel(MockDataService mockDataService)
    : ReactiveObject, IInitializeAsync, ITicketCommandSender,
        ITicketRequestSender, ILocalTicketNotificationPublisher, IStreamClient
{
    private ObservableCollection<TicketClientModel> _tickets = [];

    public ObservableCollection<TicketClientModel> Tickets
    {
        get => _tickets;
        set => this.RaiseAndSetIfChanged(ref _tickets, value);
    }

    public InitializationType Type => InitializationType.MockServices;

    public Task InitializeAsync()
    {
        Tickets = new ObservableCollection<TicketClientModel>(mockDataService.Tickets);
        return Task.CompletedTask;
    }

    public Task<bool> Send(CreateTicketCommandProto command)
    {
        _createQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(UpdateTicketDataCommandProto command)
    {
         _updateDataQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(UpdateTicketDocumentationCommandProto command)
    {
        _updateDocQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<TicketListProto> Send(GetAllTicketsRequestProto request)
    {
        var list = new TicketListProto();

        foreach (var ticket in Tickets)
        {
            list.Tickets.Add(new TicketProto()
            {
                TicketId = ticket.TicketId.ToString(),
                BookingNumber = ticket.BookingNumber,
                Documentation = ticket.Documentation,
                Name = ticket.Name,
                SprintIds = { ticket.SprintIds.ToRepeatedField() }
            });
        }

        return Task.FromResult(list);
    }

    public Task<TicketListProto> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        var list = new TicketListProto();

        // TODO add current sprint

        foreach (var ticket in Tickets.Where(t => t.SprintIds.Contains(Empty)))
        {
            list.Tickets.Add(new TicketProto()
            {
                TicketId = ticket.TicketId.ToString(),
                BookingNumber = ticket.BookingNumber,
                Documentation = ticket.Documentation,
                Name = ticket.Name,
                SprintIds = { ticket.SprintIds.ToRepeatedField() }
            });
        }

        return Task.FromResult(list);
    }

    public Task<TicketListProto> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        // TODO this is not needed anymore.
        throw new NotImplementedException();
    }

    public Task<TicketProto> Send(GetTicketByIdRequestProto request)
    {
        var ticket = Tickets.FirstOrDefault(t => t.TicketId == Parse(request.TicketId));

        if (ticket == null)
        {
            throw new Exception("Ticket not found");
        }

        var result = new TicketProto()
        {
            TicketId = ticket.TicketId.ToString(),
            BookingNumber = ticket.BookingNumber,
            Documentation = ticket.Documentation,
            Name = ticket.Name,
            SprintIds = { ticket.SprintIds.ToRepeatedField() }
        };

        return Task.FromResult(result);
    }

    public event Func<ClientTicketDataUpdatedNotification, Task>? TicketDataUpdatedNotificationReceived;

    public event Func<ClientTicketDocumentationUpdatedNotification, Task>?
        TicketDocumentationUpdatedNotificationReceived;

    public event Func<NewTicketMessage, Task>? NewTicketNotificationReceived;

    public async Task Publish(NewTicketMessage message)
    {
        if (NewTicketNotificationReceived is { } handler)
            await handler(message);
    }

    public async Task Publish(ClientTicketDataUpdatedNotification notification)
    {
        if (TicketDataUpdatedNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task Publish(ClientTicketDocumentationUpdatedNotification notification)
    {
        if (TicketDocumentationUpdatedNotificationReceived is { } handler)
            await handler(notification);
    }

    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);
    private readonly ConcurrentQueue<CreateTicketCommandProto> _createQueue = new();
    private readonly ConcurrentQueue<UpdateTicketDocumentationCommandProto> _updateDocQueue = new();
    private readonly ConcurrentQueue<UpdateTicketDataCommandProto> _updateDataQueue = new();

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_updateDocQueue.TryDequeue(out var updateDocCmd))
            {
                var notification = new ClientTicketDocumentationUpdatedNotification(
                    Parse(updateDocCmd.TicketId),
                    updateDocCmd.Documentation,
                    Parse(updateDocCmd.TraceData.TraceId)
                );

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var ticket = Tickets.FirstOrDefault(t => t.TicketId == Parse(updateDocCmd.TicketId));
                    if (ticket != null)
                        ticket.Documentation = updateDocCmd.Documentation;
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            while (_createQueue.TryDequeue(out var createCmd))
            {
                var message = new NewTicketMessage(
                    new TicketClientModel(Parse(createCmd.TicketId), createCmd.Name, createCmd.BookingNumber,
                        string.Empty, createCmd.SprintIds.Select(Parse).ToList()),
                    Parse(createCmd.TraceData.TraceId)
                );

                await Dispatcher.UIThread.InvokeAsync(() => Tickets.Add(message.Ticket));

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(message);
            }

            while (_updateDataQueue.TryDequeue(out var updateTicketDataCommandProto))
            {
                var ticketDataUpdateNotification = new ClientTicketDataUpdatedNotification(
                    Parse(updateTicketDataCommandProto.TicketId),
                    updateTicketDataCommandProto.Name, updateTicketDataCommandProto.BookingNumber,
                    updateTicketDataCommandProto.SprintIds.Select(Parse).ToList(),
                    Parse(updateTicketDataCommandProto.TraceData.TraceId));

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var ticket =
                        Tickets.FirstOrDefault(t => t.TicketId == Parse(updateTicketDataCommandProto.TicketId));
                    ticket?.Apply(ticketDataUpdateNotification);
                });

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(ticketDataUpdateNotification);
            }

            if (_updateDocQueue.IsEmpty && _createQueue.IsEmpty && _updateDataQueue.IsEmpty)
                await Task.Delay(50, cancellationToken);
        }
    }
}