using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Notifications.Note.Receiver;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockNoteCommandSender : INoteCommandSender, ILocalNoteNotificationPublisher, IStreamClient
{
    private readonly ConcurrentQueue<ClientCreateNoteCommand> _newNoteQueue = new();
    private readonly ConcurrentQueue<ClientUpdateNoteCommand> _updateNoteQueue = new();
    private readonly TimeSpan _responseDelay = TimeSpan.FromMilliseconds(50);

    public event Func<NewNoteMessage, Task>? NewNoteMessageReceived;
    public event Func<ClientNoteUpdatedNotification, Task>? ClientNoteUpdatedNotificationReceived;

    public Task<bool> Send(ClientCreateNoteCommand command)
    {
        _newNoteQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public Task<bool> Send(ClientUpdateNoteCommand command)
    {
        _updateNoteQueue.Enqueue(command);
        return Task.FromResult(true);
    }

    public async Task Publish(NewNoteMessage message)
    {
        if (NewNoteMessageReceived is { } handler)
            await handler(message);
    }

    public async Task Publish(ClientNoteUpdatedNotification notification)
    {
        if (ClientNoteUpdatedNotificationReceived is { } handler)
            await handler(notification);
    }

    public async Task StartListening(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            while (_newNoteQueue.TryDequeue(out var createCommand))
            {
                var note = new NoteClientModel(
                    createCommand.NoteId,
                    createCommand.Text,
                    createCommand.NoteTypeId,
                    createCommand.TimeSlotId,
                    createCommand.TicketId,
                    createCommand.TimeStamp
                );

                var message = new NewNoteMessage(note, createCommand.TraceId);
                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(message);
            }

            while (_updateNoteQueue.TryDequeue(out var updateCommand))
            {
                var notification = new ClientNoteUpdatedNotification(
                    updateCommand.NoteId,
                    updateCommand.Text,
                    updateCommand.NoteTypeId,
                    updateCommand.TimeSlotId,
                    updateCommand.TraceId
                );

                await Task.Delay(_responseDelay, cancellationToken);
                await Publish(notification);
            }

            if (_newNoteQueue.IsEmpty && _updateNoteQueue.IsEmpty)
            {
                await Task.Delay(50, cancellationToken);
            }
        }
    }
}
