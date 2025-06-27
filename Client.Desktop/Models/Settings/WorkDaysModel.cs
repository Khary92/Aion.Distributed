using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using Client.Desktop.Services;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Google.Protobuf.WellKnownTypes;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Proto.Command.WorkDays;
using Proto.Notifications.UseCase;
using Proto.Requests.WorkDays;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class WorkDaysModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    IRunTimeSettings runTimeSettings) : ReactiveObject
{
    public ObservableCollection<WorkDayDto> WorkDays { get; } = [];

    public void RegisterMessenger()
    {
        messenger.Register<NewWorkDayMessage>(this, (_, m) =>
        {
            //tracer.WorkDay.Create.AggregateReceived(GetType(), m.WorkDay.WorkDayId, m.WorkDay.AsTraceAttributes());
            WorkDays.Add(m.WorkDay);
            //tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

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

        //tracer.WorkDay.Create.CommandSent(GetType(), newGuid, createWorkDayCommand);
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
        runTimeSettings.SelectedDate = selectedWorkDay.Date;

        var workDaySelectionChangedNotification = new WorkDaySelectionChangedNotification();

        //TODO This is wrong. Fix it
        //mediator.Publish(workDaySelectionChangedNotification);
        //TODO hmmm how to trace simple Events?
        //logger.LogNotificationPublished(workDaySelectionChangedNotification);
    }
}