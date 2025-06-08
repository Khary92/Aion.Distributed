using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using Client.Avalonia.Communication.RequiresChange;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Export;

public class ExportModel : ReactiveObject
{
    private readonly IExportService _exportService;
    private readonly IMediator _mediator;
    private readonly IMessenger _messenger;
    private readonly ITracingCollectorProvider _tracer;

    private string _markdownText = null!;

    public ExportModel(IMediator mediator, IMessenger messenger, IExportService exportService,
        ITracingCollectorProvider tracer)
    {
        _mediator = mediator;
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
        _messenger.Register<NewWorkDayMessage>(this, (_, m) =>
        {
            _tracer.WorkDay.Create.AggregateReceived(GetType(), m.WorkDay.WorkDayId, m.WorkDay.AsTraceAttributes());
            WorkDays.Add(m.WorkDay);
            _tracer.WorkDay.Create.AggregateAdded(GetType(), m.WorkDay.WorkDayId);
        });
    }

    public async Task InitializeAsync()
    {
        WorkDays.Clear();
        WorkDays.AddRange(await _mediator.Send(new GetAllWorkDaysRequest()));
    }

    public async Task<bool> ExportFileAsync()
    {
        if (await _mediator.Send(new IsExportPathValidRequest())) return await _exportService.ExportToFile(WorkDays);

        _tracer.Export.ToFile.PathSettingsInvalid(GetType(), new IsExportPathValidRequest());
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
            _tracer.Export.ToFile.ExceptionOccured(GetType(), exception);
        }
    }

    public async Task<string> GetMarkdownTextAsync()
    {
        return await _exportService.GetMarkdownString(SelectedWorkDays);
    }
}