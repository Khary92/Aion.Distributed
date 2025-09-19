using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.Export;
using Client.Desktop.Services.LocalSettings;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Export;

public class ExportModel(
    IMessenger messenger,
    IRequestSender requestSender,
    IExportService exportService,
    ITraceCollector tracer,
    ILocalSettingsService settingsService)
    : ReactiveObject, IInitializeAsync, IMessengerRegistration, IRecipient<NewWorkDayMessage>
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
        messenger.RegisterAll(this);
    }

    public void UnregisterMessenger()
    {
        messenger.UnregisterAll(this);
    }

    public void Receive(NewWorkDayMessage message)
    {
        WorkDays.Add(message.WorkDay);
        _ = tracer.WorkDay.Create.AggregateAdded(GetType(), message.WorkDay.WorkDayId);
    }

    public async Task<bool> ExportFileAsync()
    {
        if (settingsService.IsExportPathValid()) return await exportService.ExportToFile(WorkDays);

        return false;
    }

    private void RefreshMarkdownViewerHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _ = GetMarkdownTextAsync();
    }

    public async Task<string> GetMarkdownTextAsync()
    {
        return await exportService.GetMarkdownString(SelectedWorkDays);
    }
}