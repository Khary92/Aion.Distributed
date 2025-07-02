using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Converter;
using Client.Desktop.DTO;
using Client.Desktop.Services;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Requests.Settings;
using Proto.Requests.WorkDays;
using ReactiveUI;

namespace Client.Desktop.Models.Export;

public class ExportModel : ReactiveObject
{
    private readonly IExportService _exportService;
    private readonly IMessenger _messenger;

    private readonly IRequestSender _requestSender;
    private readonly ITraceCollector _tracer;

    private string _markdownText = null!;

    public ExportModel(IRequestSender requestSender, IMessenger messenger,
        IExportService exportService, ITraceCollector tracer)
    {
        _requestSender = requestSender;
        _messenger = messenger;
        _exportService = exportService;
        _tracer = tracer;

        SelectedWorkDays.CollectionChanged += RefreshMarkdownViewerHandler;
    }

    public ObservableCollection<WorkDayDto> WorkDays { get; } = [];
    public ObservableCollection<WorkDayDto> SelectedWorkDays { get; } = [];

    public string MarkdownText
    {
        get => _markdownText;
        set => this.RaiseAndSetIfChanged(ref _markdownText, value);
    }

    public void RegisterMessenger()
    {
        _messenger.Register<NewWorkDayMessage>(this, async (_, m) =>
        {
            await _tracer.WorkDay.Create.AggregateReceived(GetType(), m.WorkDay.WorkDayId,
                m.WorkDay.AsTraceAttributes());
            WorkDays.Add(m.WorkDay);
            await _tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

    public async Task InitializeAsync()
    {
        WorkDays.Clear();
        WorkDays.AddRange(await _requestSender.Send(new GetAllWorkDaysRequestProto()));
    }

    public async Task<bool> ExportFileAsync()
    {
        var isExportPathValidRequestProto = new IsExportPathValidRequestProto();
        if (await _requestSender.Send(isExportPathValidRequestProto))
            return await _exportService.ExportToFile(WorkDays);

        await _tracer.Export.ToFile.PathSettingsInvalid(GetType(), isExportPathValidRequestProto);
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
            await _tracer.Export.ToFile.ExceptionOccured(GetType(), exception);
        }
    }

    public async Task<string> GetMarkdownTextAsync()
    {
        return await _exportService.GetMarkdownString(SelectedWorkDays);
    }
}