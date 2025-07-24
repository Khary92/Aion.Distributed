using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Converter;
using Client.Desktop.DTO;
using Client.Desktop.Services.Initializer;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Google.Protobuf.WellKnownTypes;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Proto.Command.WorkDays;
using Proto.Requests.WorkDays;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class WorkDaysModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    ILocalSettingsCommandSender localSettingsCommandSender,
    IMessenger messenger,
    ITraceCollector tracer) : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    public ObservableCollection<WorkDayDto> WorkDays { get; } = [];

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

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        if (!await requestSender.Send(new IsWorkDayExistingRequestProto
            {
                Date = DateTimeOffset.Now.ToTimestamp()
            }))
            await commandSender.Send(new CreateWorkDayCommandProto
            {
                WorkDayId = Guid.NewGuid().ToString(),
                Date = DateTimeOffset.Now.ToTimestamp()
            });

        WorkDays.Clear();
        WorkDays.AddRange(await requestSender.Send(new GetAllWorkDaysRequestProto()));
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
        var createWorkDayCommand = new CreateWorkDayCommandProto
        {
            WorkDayId = newGuid.ToString(),
            Date = Timestamp.FromDateTimeOffset(date)
        };
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

    public void SetSelectedWorkday(WorkDayDto selectedWorkDay)
    {
        localSettingsCommandSender.Send(new SetWorkDaySelectionCommand(selectedWorkDay.Date));
    }
}