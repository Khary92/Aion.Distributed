using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.WorkDay;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.RequiresChange;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using DynamicData;
using Proto.Requests.Settings;
using Proto.Requests.WorkDays;
using ReactiveUI;

namespace Client.Desktop.Models.Export;

public class ExportModel : ReactiveObject
{
    private readonly IExportService _exportService;
    private readonly IRequestSender _requestSender;
    private readonly IMessenger _messenger;
    //private readonly ITracingCollectorProvider _tracer;

    private string _markdownText = null!;

    public ExportModel(IRequestSender requestSender, IMessenger messenger,
        IExportService exportService)
    {
        _requestSender = requestSender;
        _messenger = messenger;
        _exportService = exportService;

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
        _messenger.Register<NewWorkDayMessage>(this, (_, m) =>
        {
            //_tracer.WorkDay.Create.AggregateReceived(GetType(), m.WorkDay.WorkDayId, m.WorkDay.AsTraceAttributes());
            WorkDays.Add(m.WorkDay);
            //_tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

    public async Task InitializeAsync()
    {
        WorkDays.Clear();
        WorkDays.AddRange(await _requestSender.Send(new GetAllWorkDaysRequestProto()));
    }

    public async Task<bool> ExportFileAsync()
    {
        if (await _requestSender.Send(new IsExportPathValidRequestProto()))
            return await _exportService.ExportToFile(WorkDays);

        //_tracer.Export.ToFile.PathSettingsInvalid(GetType(), new IsExportPathValidRequest());
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
            //_tracer.Export.ToFile.ExceptionOccured(GetType(), exception);
        }
    }

    public async Task<string> GetMarkdownTextAsync()
    {
        return await _exportService.GetMarkdownString(SelectedWorkDays);
    }
}