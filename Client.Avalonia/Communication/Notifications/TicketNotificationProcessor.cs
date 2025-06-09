using System.Threading;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Tickets;
using Contract.DTO;
using MediatR;

namespace Client.Avalonia.Communication.NotificationProcessors;

public class TicketNotificationProcessor(IMessenger messenger) :
    INotificationHandler<TicketCreatedNotification>,
    INotificationHandler<TicketDataUpdatedNotification>,
    INotificationHandler<TicketDocumentationUpdatedNotification>
{
    public Task Handle(TicketCreatedNotification command, CancellationToken cancellationToken)
    {
        var ticket = new TicketDto(command.TicketId, command.Name, command.BookingNumber, string.Empty,
            command.SprintIds);

        messenger.Send(new NewTicketMessage(ticket));

        return Task.CompletedTask;
    }

    public Task Handle(TicketDataUpdatedNotification command, CancellationToken cancellationToken)
    {
        messenger.Send(command);

        return Task.CompletedTask;
    }

    public Task Handle(TicketDocumentationUpdatedNotification command, CancellationToken cancellationToken)
    {
        messenger.Send(command);

        return Task.CompletedTask;
    }
}