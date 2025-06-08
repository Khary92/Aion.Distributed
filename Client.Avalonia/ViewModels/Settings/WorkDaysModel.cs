using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using Client.Avalonia.Communication.RequiresChange;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.UseCase;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using MediatR;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Settings;

public class WorkDaysModel(
    IMediator mediator,
    IMessenger messenger,
    IRunTimeSettings runTimeSettings,
    ITracingCollectorProvider tracer) : ReactiveObject
{
    public ObservableCollection<WorkDayDto> WorkDays { get; } = [];

    public void RegisterMessenger()
    {
        messenger.Register<NewWorkDayMessage>(this, (_, m) =>
        {
            tracer.WorkDay.Create.AggregateReceived(GetType(), m.WorkDay.WorkDayId, m.WorkDay.AsTraceAttributes());
            WorkDays.Add(m.WorkDay);
            tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

    public async Task InitializeAsync()
    {
        WorkDays.Clear();
        WorkDays.AddRange(await mediator.Send(new GetAllWorkDaysRequest()));
    }

    public async Task AddWorkDayAsync(DateTimeOffset date)
    {
        var existingWorkDay = await mediator.Send(new GetWorkDayByDateRequest(date));

        if (existingWorkDay != null)
        {
            await ShowMessageBox("Warning", "This date already exists");
            return;
        }

        var createWorkDayCommand = new CreateWorkDayCommand(Guid.NewGuid(), date);
        await mediator.Send(createWorkDayCommand);

        tracer.WorkDay.Create.CommandSent(GetType(), createWorkDayCommand.WorkDayId, createWorkDayCommand);
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

        mediator.Publish(workDaySelectionChangedNotification);
        //TODO hmmm how to trace simple Events?
        //logger.LogNotificationPublished(workDaySelectionChangedNotification);
    }
}