using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using ReactiveUI;

namespace Client.Desktop.Replays;

public class TicketReplayDecorator : ReactiveObject
{
    private readonly List<DocumentationReplayDto> _documentationHistory = [];

    private readonly IRequestSender _requestSender;
    // private readonly IHistoryLoader<DocumentationReplayDto> _documentationHistoryLoader;

    private string _displayedDocumentation = string.Empty;

    private int _index;

    private bool _isReplayMode;

    public TicketReplayDecorator(IRequestSender requestSender, TicketDto ticketDto)
    {
        _requestSender = requestSender;
        Ticket = ticketDto;

        DisplayedDocumentation = ticketDto.Documentation;
    }

    public TicketDto Ticket { get; init; }

    public string DisplayedDocumentation
    {
        get => _displayedDocumentation;
        set
        {
            if (!IsReplayMode) Ticket.Documentation = value;

            this.RaiseAndSetIfChanged(ref _displayedDocumentation, value);
        }
    }

    public bool IsReplayMode
    {
        get => _isReplayMode;
        set => this.RaiseAndSetIfChanged(ref _isReplayMode, value);
    }

    public async Task LoadHistory()
    {
//        var ticketDocumentationEventsByTicketId =
//            await _documentationHistoryLoader.Load(Ticket.TicketId);

//        _documentationHistory.Clear();
//        foreach (var ticketDocumentationEvent in ticketDocumentationEventsByTicketId)
//            _documentationHistory.Add(new DocumentationReplayDto(ticketDocumentationEvent.Documentation));

//        if (_documentationHistory.Count == 0) return;

//        _index = 0;
//        DisplayedDocumentation = _documentationHistory[_index].Documentation;
    }

    public void ExitReplay()
    {
        IsReplayMode = false;
        DisplayedDocumentation = Ticket.Documentation;
    }

    public void Next()
    {
        if (_index >= _documentationHistory.Count - 1) return;
        _index++;
        DisplayedDocumentation = _documentationHistory[_index].Documentation;
    }

    public void Previous()
    {
        if (_index <= 0) return;
        _index--;
        DisplayedDocumentation = _documentationHistory[_index].Documentation;
    }
}