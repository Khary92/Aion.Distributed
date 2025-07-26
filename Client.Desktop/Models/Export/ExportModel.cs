using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Converter;
using Client.Desktop.DTO;
using Client.Desktop.DTO.Local;
using Client.Desktop.Services;
using Client.Desktop.Services.Initializer;
using Client.Desktop.Services.LocalSettings;
using Client.Desktop.Services.LocalSettings.Commands;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Requests.WorkDays;
using ReactiveUI;

namespace Client.Desktop.Models.Export;

public class ExportModel(
    IRequestSender requestSender,
    IMessenger messenger,
    IExportService exportService,
    ITraceCollector tracer,
    ILocalSettingsService localSettingsService)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private string _markdownText = null!;
    private SettingsDto? Settings { get; set; }

    public ObservableCollection<WorkDayDto> WorkDays { get; } = [];
    public ObservableCollection<WorkDayDto> SelectedWorkDays { get; } = [];

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
        WorkDays.AddRange(await requestSender.Send(new GetAllWorkDaysRequestProto()));
    }

    public void RegisterMessenger()
    {
        messenger.Register<ExportPathSetNotification>(this,
            async void (_, m) => { Settings!.ExportPath = m.ExportPath; });

        messenger.Register<SettingsDto>(this, (_, m) => { Settings = m; });

        messenger.Register<NewWorkDayMessage>(this, async void (_, m) =>
        {
            await tracer.WorkDay.Create.AggregateReceived(GetType(), m.WorkDay.WorkDayId,
                m.WorkDay.AsTraceAttributes());
            WorkDays.Add(m.WorkDay);
            await tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

    public async Task<bool> ExportFileAsync()
    {
        if (localSettingsService.IsExportPathValid()) return await exportService.ExportToFile(WorkDays);

        await tracer.Export.ToFile.PathSettingsInvalid(GetType(), Settings!.ExportPath);
        return false;
    }

    private async void RefreshMarkdownViewerHandler(object? sender, NotifyCollectionChangedEventArgs e)
    {
        try
        {
            await GetMarkdownTextAsync();
        }
        catch (Exception exception)
        {
            await tracer.Export.ToFile.ExceptionOccured(GetType(), exception);
        }
    }

    public async Task<string> GetMarkdownTextAsync()
    {
        return await exportService.GetMarkdownString(SelectedWorkDays);
    }
}