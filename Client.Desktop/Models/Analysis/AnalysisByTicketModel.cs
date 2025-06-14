using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.RequiresChange;
using Contract.Decorators;
using Contract.DTO;
using DynamicData;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using ReactiveUI;

namespace Client.Desktop.Models.Analysis;

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
        Tickets.AddRange(await _requestSender.Send(new GetTicketsWithShowAllSwitchRequestProto { IsShowAll = false }));
    }

    public async Task SetAnalysisByTicket(TicketDto selectedTicket)
    {
        AnalysisByTicket = await _analysisDataService.GetAnalysisByTicket(selectedTicket);
    }

    public async Task<TagDto> GetTagById(Guid tagId)
    {
        return await _requestSender.Send(new GetTagByIdRequestProto
        {
            TagId = tagId.ToString()
        });
    }
}