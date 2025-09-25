using System;
using System.Collections.Generic;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Presentation.Models.Synchronization;
using ReactiveUI;

namespace Client.Desktop.DataModels;

public class TicketClientModel : ReactiveObject, IDocumentationSynchronizationListener
{
    private readonly Guid _ticketId;
    private string _bookingNumber = string.Empty;
    private string _documentation = string.Empty;
    private string _name = string.Empty;
    private List<Guid> _sprintIds = [];

    public TicketClientModel(Guid ticketId, string name, string bookingNumber, string documentation,
        List<Guid> sprintIds)
    {
        TicketId = ticketId;
        Name = name;
        BookingNumber = bookingNumber;
        Documentation = documentation;
        SprintIds = sprintIds;
    }

    public bool IsDirty { get; private set; }

    private IDocumentationSynchronizer? _documentationSynchronizer;
    
    public IDocumentationSynchronizer? DocumentationSynchronizer
    {
        get => _documentationSynchronizer;
        set
        {
            this.RaiseAndSetIfChanged(ref _documentationSynchronizer, value);
            _documentationSynchronizer?.Register(this);
        }
    }
    
    public Guid TicketId
    {
        get => _ticketId;
        private init => this.RaiseAndSetIfChanged(ref _ticketId, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string BookingNumber
    {
        get => _bookingNumber;
        set => this.RaiseAndSetIfChanged(ref _bookingNumber, value);
    }

    public List<Guid> SprintIds
    {
        get => _sprintIds;
        private set => this.RaiseAndSetIfChanged(ref _sprintIds, value);
    }

    public string Documentation
    {
        get => _documentation;
        set
        {
            if (string.Equals(_documentation, value, StringComparison.Ordinal)) return;
            this.RaiseAndSetIfChanged(ref _documentation, value);
            DocumentationSynchronizer?.Synchronize(_ticketId, value);
            IsDirty = true;
        }
    }

    public void Apply(ClientTicketDataUpdatedNotification notification)
    {
        BookingNumber = notification.BookingNumber;
        Name = notification.Name;
        SprintIds = notification.SprintIds;
    }

    public void Apply(ClientTicketDocumentationUpdatedNotification notification)
    {
        Documentation = notification.Documentation;
        IsDirty = false;
    }
    
    public TicketClientModel Ticket => this;
}