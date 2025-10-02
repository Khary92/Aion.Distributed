using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Presentation.Models.Replay;

public interface ITicketReplayProvider
{
    event Action<DocumentationReplay>? DocumentationChanged;
    Task LoadHistory(Guid ticketId);
    void Next();
    void Previous();
}