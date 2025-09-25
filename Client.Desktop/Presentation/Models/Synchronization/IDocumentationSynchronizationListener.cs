using Client.Desktop.DataModels;

namespace Client.Desktop.Presentation.Models.Synchronization;

public interface IDocumentationSynchronizationListener
{
    public TicketClientModel Ticket { get; }
}