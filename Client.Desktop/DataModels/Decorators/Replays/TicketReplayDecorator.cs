using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.Communication.Requests.Ticket;
using ReactiveUI;

namespace Client.Desktop.DataModels.Decorators.Replays;

public class TicketReplayDecorator : ReactiveObject
{
    private readonly List<DocumentationReplay> _documentationHistory = [];

    private readonly IRequestSender _requestSender;

    private string _displayedDocumentation = string.Empty;

    private int _index;

    private bool _isReplayMode;

    public TicketReplayDecorator(IRequestSender requestSender, TicketClientModel ticketClientModel)
    {
        _requestSender = requestSender;
        Ticket = ticketClientModel;

        DisplayedDocumentation = ticketClientModel.Documentation;
    }

    public TicketClientModel Ticket { get; init; }

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
        var ticketDocumentationEventsByTicketId =
            await _requestSender.Send(new ClientGetTicketReplaysByIdRequest(Ticket.TicketId, Guid.NewGuid()));

        _documentationHistory.Clear();
        foreach (var ticketDocumentationEvent in ticketDocumentationEventsByTicketId)
            _documentationHistory.Add(new DocumentationReplay(ticketDocumentationEvent.Documentation));

        if (_documentationHistory.Count == 0) return;

        _index = 0;
        DisplayedDocumentation = _documentationHistory[_index].Documentation;
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