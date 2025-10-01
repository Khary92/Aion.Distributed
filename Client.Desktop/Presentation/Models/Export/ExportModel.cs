using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.Export;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Export;

public class ExportModel(
    IRequestSender requestSender,
    IExportService exportService,
    ITraceCollector tracer,
    ILocalSettingsService settingsService,
    INotificationPublisherFacade notificationPublisher)
    : ReactiveObject, IInitializeAsync, IMessengerRegistration
{
    private string _markdownText = string.Empty;

    public ObservableCollection<WorkDayClientModel> WorkDays { get; } = [];
    public ObservableCollection<WorkDayClientModel> SelectedWorkDays { get; } = [];

    public string MarkdownText
    {
        get => _markdownText;
        set => this.RaiseAndSetIfChanged(ref _markdownText, value);
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        SelectedWorkDays.CollectionChanged += RefreshMarkdownViewerHandler;

        WorkDays.Clear();
        WorkDays.AddRange(await requestSender.Send(new ClientGetAllWorkDaysRequest(Guid.NewGuid())));
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

    public async Task<bool> ExportFileAsync()
    {
        if (settingsService.IsExportPathValid()) return await exportService.ExportToFile(WorkDays);

        return false;
    }

    // async void is bad but it should work for now
    public async void RefreshMarkdownViewerHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        await Dispatcher.UIThread.InvokeAsync(async () =>
            MarkdownText = await exportService.GetMarkdownString(SelectedWorkDays));
    }
}