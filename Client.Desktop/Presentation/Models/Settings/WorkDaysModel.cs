using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Initialize;
using Client.Desktop.Lifecycle.Startup.Register;
using Client.Desktop.Services;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Google.Protobuf.WellKnownTypes;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Proto.Requests.WorkDays;
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
        if (!await requestSender.Send(new IsWorkDayExistingRequestProto
            {
                Date = DateTimeOffset.Now.ToTimestamp()
            }))
            await commandSender.Send(new ClientCreateWorkDayCommand(Guid.NewGuid(), DateTimeOffset.Now));

        WorkDays.Clear();
        WorkDays.AddRange(await requestSender.Send(new GetAllWorkDaysRequestProto()));
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

    public async Task AddWorkDayAsync(DateTimeOffset date)
    {
        var existingWorkDay = await requestSender.Send(new GetWorkDayByDateRequestProto
        {
            Date = Timestamp.FromDateTimeOffset(date)
        });

        //TODO There is something wrong
        if (existingWorkDay != null)
        {
            await ShowMessageBox("Warning", "This date already exists");
            return;
        }

        var newGuid = Guid.NewGuid();
        var createWorkDayCommand = new ClientCreateWorkDayCommand(newGuid, date);
        await commandSender.Send(createWorkDayCommand);

        await tracer.WorkDay.Create.CommandSent(GetType(), newGuid, createWorkDayCommand);
    }

    private static async Task ShowMessageBox(string title, string message)
    {
        await MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = title,
            ContentMessage = message,
            ButtonDefinitions = ButtonEnum.Ok,
            Icon = Icon.Info
        }).ShowAsync();
    }

    public void SetSelectedWorkday(WorkDayClientModel selectedWorkDay)
    {
        localSettingsCommandSender.Send(new SetWorkDaySelectionCommand(selectedWorkDay.Date));
    }
}