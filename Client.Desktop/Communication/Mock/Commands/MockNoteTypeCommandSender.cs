using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NoteType.Receiver;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Proto.Command.NoteTypes;
using Service.Proto.Shared.Commands.NoteTypes;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockNoteTypeCommandSender
    : INoteTypeCommandSender, ILocalNoteTypeNotificationPublisher, IStreamClient
{
    public event Func<ClientNoteTypeColorChangedNotification, Task>? ClientNoteTypeColorChangedNotificationReceived;
    public event Func<ClientNoteTypeNameChangedNotification, Task>? ClientNoteTypeNameChangedNotificationReceived;
    public event Func<NewNoteTypeMessage, Task>? NewNoteTypeMessageReceived;

    public Task<bool> Send(CreateNoteTypeCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(ChangeNoteTypeNameCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task<bool> Send(ChangeNoteTypeColorCommandProto command)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientNoteTypeNameChangedNotification notification)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(ClientNoteTypeColorChangedNotification notification)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task Publish(NewNoteTypeMessage message)
    {
        // Do nothing. The client does not manage these kinds of events
        throw new NotImplementedException();
    }

    public Task StartListening(CancellationToken cancellationToken)
    {
        // Do nothing. The client does not manage these kinds of events
        return Task.FromResult(true);
    }
}