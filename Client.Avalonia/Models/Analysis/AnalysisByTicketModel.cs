using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Requests;
using Client.Avalonia.Communication.RequiresChange;
using Contract.Decorators;
using Contract.DTO;
using DynamicData;
using ReactiveUI;

namespace Client.Avalonia.Models.Analysis;

public class AnalysisByTicketModel : ReactiveObject
{
    private readonly IRequestSender _requestSender;
    private readonly IAnalysisDataService _analysisDataService;

    private AnalysisByTicketDecorator? _analysisByTicket;

    public AnalysisByTicketModel(IRequestSender requestSender, IAnalysisDataService analysisDataService)
    {
        _requestSender = requestSender;
        _analysisDataService = analysisDataService;
        _analysisDataService = analysisDataService;

        InitializeAsync().ConfigureAwait(false);
    }

    public ObservableCollection<TicketDto> Tickets { get; } = [];

    public AnalysisByTicketDecorator? AnalysisByTicket
    {
        get => _analysisByTicket;
        private set => this.RaiseAndSetIfChanged(ref _analysisByTicket, value);
    }

    private async Task InitializeAsync()
    {
        Tickets.Clear();
        Tickets.AddRange(await _requestSender.GetTicketsWithShowAllSwitch(false));
    }

    public async Task SetAnalysisByTicket(TicketDto selectedTicket)
    {
        AnalysisByTicket = await _analysisDataService.GetAnalysisByTicket(selectedTicket);
    }

    public async Task<TagDto> GetTagById(Guid tagId)
    {
        return await _requestSender.GetTagById(tagId);
    }
}