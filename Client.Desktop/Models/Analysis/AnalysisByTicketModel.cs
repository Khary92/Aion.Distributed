using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Decorators;
using Client.Desktop.DTO;
using DynamicData;
using Proto.Requests.AnalysisData;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using ReactiveUI;

namespace Client.Desktop.Models.Analysis;

public class AnalysisByTicketModel : ReactiveObject
{
    private readonly IRequestSender _requestSender;

    private AnalysisByTicketDecorator? _analysisByTicket;

    public AnalysisByTicketModel(IRequestSender requestSender)
    {
        _requestSender = requestSender;

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
        Tickets.AddRange(await _requestSender.Send(new GetTicketsWithShowAllSwitchRequestProto { IsShowAll = true }));
    }

    public async Task SetAnalysisByTicket(TicketDto selectedTicket)
    {
        AnalysisByTicket = await _requestSender.Send(new GetTicketAnalysisById
        {
            TicketId = selectedTicket.TicketId.ToString()
        });
    }

    public async Task<TagDto> GetTagById(Guid tagId)
    {
        return await _requestSender.Send(new GetTagByIdRequestProto
        {
            TagId = tagId.ToString()
        });
    }
}