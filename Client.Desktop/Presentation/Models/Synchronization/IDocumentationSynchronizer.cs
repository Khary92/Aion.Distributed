using System;

namespace Client.Desktop.Presentation.Models.Synchronization;

public interface IDocumentationSynchronizer
{
    void Register(IDocumentationSynchronizationListener listener);
    void Synchronize(Guid ticketId, string documentation);
}