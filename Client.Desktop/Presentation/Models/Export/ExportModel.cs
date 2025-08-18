using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Local;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Services.Export;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Export;

public class ExportModel(
    IRequestSender requestSender,
    IMessenger messenger,
    IExportService exportService,
    ITraceCollector tracer,
    ILocalSettingsService localSettingsService)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private string _markdownText = null!;
    private SettingsClientModel? Settings { get; set; }

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
        messenger.Register<ExportPathSetNotification>(this, void (_, m) => { Settings!.ExportPath = m.ExportPath; });

        messenger.Register<SettingsClientModel>(this, (_, m) => { Settings = m; });

        messenger.Register<NewWorkDayMessage>(this, async void (_, m) =>
        {
            WorkDays.Add(m.WorkDay);
            await tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

    public async Task<bool> ExportFileAsync()
    {
        if (localSettingsService.IsExportPathValid()) return await exportService.ExportToFile(WorkDays);

        return false;
    }

    private async void RefreshMarkdownViewerHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        await GetMarkdownTextAsync();
    }

    public async Task<string> GetMarkdownTextAsync()
    {
        return await exportService.GetMarkdownString(SelectedWorkDays);
    }
}