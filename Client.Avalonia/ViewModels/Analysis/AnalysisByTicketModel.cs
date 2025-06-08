using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Avalonia.Communication.RequiresChange;
using Contract.Decorators;
using Contract.DTO;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Analysis;

public class AnalysisByTicketModel : ReactiveObject
{
    private readonly IAnalysisDataService _analysisDataService;
    private readonly IMediator _mediator;

    private AnalysisByTicketDecorator? _analysisByTicket;

    public AnalysisByTicketModel(IMediator mediator, IAnalysisDataService analysisDataService)
    {
        _mediator = mediator;
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
        Tickets.AddRange(await _mediator.Send(new GetTicketsWithShowAllSwitchRequest(false)));
    }

    public async Task SetAnalysisByTicket(TicketDto selectedTicket)
    {
        AnalysisByTicket = await _analysisDataService.GetAnalysisByTicket(selectedTicket);
    }

    public async Task<TagDto> GetTagById(Guid tagId)
    {
        return await _mediator.Send(new GetTagByIdRequest(tagId));
    }
}