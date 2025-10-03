using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Presentation.Models.Replay;

public class TicketReplayProvider(IRequestSender requestSender) : ITicketReplayProvider
{
    private int _currentIndex;
    private List<DocumentationReplay> _documentationHistory = new();

    public event Action<DocumentationReplay>? DocumentationChanged;

    public async Task LoadHistory(Guid ticketId)
    {
        _documentationHistory =
            await requestSender.Send(new ClientGetTicketReplaysByIdRequest(ticketId, Guid.NewGuid()));
        _currentIndex = _documentationHistory.Count - 1;
    }

    public void Next()
    {
        if (_documentationHistory.Count - 1 == _currentIndex) return;

        _currentIndex++;

        if (DocumentationChanged != null) DocumentationChanged.Invoke(_documentationHistory[_currentIndex]);
    }

    public void Previous()
    {
        if (_currentIndex <= 0) return;

        _currentIndex--;

        if (DocumentationChanged != null) DocumentationChanged.Invoke(_documentationHistory[_currentIndex]);
    }
}