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
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
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
        WorkDays.AddRange(await requestSender.Send(new ClientGetAllWorkDaysRequest()));

        if (!await requestSender.Send(new ClientIsWorkDayExistingRequest(DateTimeOffset.Now)))
        {
            await commandSender.Send(new ClientCreateWorkDayCommand(Guid.NewGuid(), DateTimeOffset.Now,
                Guid.NewGuid()));
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

    public async Task AddWorkDayAsync(DateTimeOffset date)
    {
        var existingWorkDay = await requestSender.Send(new ClientGetWorkDayByDateRequest(DateTimeOffset.Now));

        //TODO There is something wrong
        if (existingWorkDay != null)
        {
            await ShowMessageBox("Warning", "This date already exists");
            return;
        }

        var newGuid = Guid.NewGuid();
        var createWorkDayCommand = new ClientCreateWorkDayCommand(newGuid, date, Guid.NewGuid());
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