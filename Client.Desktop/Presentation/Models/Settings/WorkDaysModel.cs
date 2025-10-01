using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.Communication.Local.LocalEvents.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Settings;

public class WorkDaysModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    ILocalSettingsService localSettingsService,
    ITraceCollector tracer,
    INotificationPublisherFacade notificationPublisher) : ReactiveObject, IInitializeAsync, IMessengerRegistration,
    IClientWorkDaySelectionChangedNotificationPublisher
{
    public ObservableCollection<WorkDayClientModel> WorkDays { get; } = [];

    public event Func<ClientWorkDaySelectionChangedNotification, Task>?
        ClientWorkDaySelectionChangedNotificationReceived;

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        var workDayClientModels = await requestSender.Send(new ClientGetAllWorkDaysRequest(Guid.NewGuid()));
        var isWorkdayAlreadyExisting =
            await requestSender.Send(new ClientIsWorkDayExistingRequest(DateTimeOffset.Now, Guid.NewGuid()));

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            WorkDays.Clear();
            WorkDays.AddRange(workDayClientModels);
        });

        var traceId = Guid.NewGuid();
        if (isWorkdayAlreadyExisting)
        {
            await tracer.WorkDay.Create.ActionAborted(GetType(), traceId);
            return;
        }

        await tracer.WorkDay.Create.StartUseCase(GetType(), traceId);
        var clientCreateWorkDayCommand = new ClientCreateWorkDayCommand(Guid.NewGuid(), DateTimeOffset.Now, traceId);
        await tracer.WorkDay.Create.SendingCommand(GetType(), traceId, clientCreateWorkDayCommand);
        await commandSender.Send(clientCreateWorkDayCommand);
    }

    public void RegisterMessenger()
    {
        notificationPublisher.WorkDay.NewWorkDayMessageReceived += HandleNewWorkDayMessage;
    }

    public void UnregisterMessenger()
    {
        notificationPublisher.WorkDay.NewWorkDayMessageReceived -= HandleNewWorkDayMessage;
    }

    private async Task HandleNewWorkDayMessage(NewWorkDayMessage message)
    {
        await Dispatcher.UIThread.InvokeAsync(() => { WorkDays.Add(message.WorkDay); });
        await tracer.WorkDay.Create.AggregateAdded(GetType(), message.TraceId);
    }

    public async Task SetSelectedWorkday(WorkDayClientModel selectedWorkDay)
    {
        if (ClientWorkDaySelectionChangedNotificationReceived == null)
            throw new InvalidOperationException(
                "Ticket data update received but no forwarding receiver is set");

        await ClientWorkDaySelectionChangedNotificationReceived.Invoke(new ClientWorkDaySelectionChangedNotification());

        await localSettingsService.SetSelectedDate(selectedWorkDay.Date);
    }
}