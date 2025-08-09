using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Settings;

public class WorkDaysModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    ILocalSettingsCommandSender localSettingsCommandSender,
    IMessenger messenger,
    ITraceCollector tracer) : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    public ObservableCollection<WorkDayClientModel> WorkDays { get; } = [];

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        WorkDays.Clear();
        WorkDays.AddRange(await requestSender.Send(new ClientGetAllWorkDaysRequest(Guid.NewGuid())));

        if (!await requestSender.Send(new ClientIsWorkDayExistingRequest(DateTimeOffset.Now, Guid.NewGuid())))
        {
            var traceId = Guid.NewGuid();
            await tracer.WorkDay.Create.StartUseCase(GetType(), traceId);

            var clientCreateWorkDayCommand = new ClientCreateWorkDayCommand(Guid.NewGuid(), DateTimeOffset.Now,
                traceId);

            await tracer.WorkDay.Create.SendingCommand(GetType(), traceId, clientCreateWorkDayCommand);
            await commandSender.Send(clientCreateWorkDayCommand);
        }
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewWorkDayMessage>(this, async void (_, m) =>
        {
            await tracer.WorkDay.Create.AggregateReceived(GetType(), m.WorkDay.WorkDayId,
                m.WorkDay.AsTraceAttributes());
            WorkDays.Add(m.WorkDay);
            await tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

    public void SetSelectedWorkday(WorkDayClientModel selectedWorkDay)
    {
        localSettingsCommandSender.Send(new SetWorkDaySelectionCommand(selectedWorkDay.Date));
    }
}