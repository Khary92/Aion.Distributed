using System;
using System.Collections.Generic;
using System.Linq;
using Proto.Notifications.Ticket;
using ReactiveUI;

namespace Client.Desktop.DTO;

public class TicketDto : ReactiveObject
{
    private readonly Guid _ticketId;
    private string _bookingNumber = string.Empty;
    private string _documentation = string.Empty;
    private string _name = string.Empty;
    private List<Guid> _sprintIds = [];

    public TicketDto(Guid ticketId, string name, string bookingNumber, string documentation, List<Guid> sprintIds)
    {
        TicketId = ticketId;
        Name = name;
        BookingNumber = bookingNumber;
        Documentation = documentation;
        SprintIds = sprintIds;
        PreviousDocumentation = documentation;
    }

    private string PreviousDocumentation { get; set; }

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
        set => this.RaiseAndSetIfChanged(ref _documentation, value);
    }

    public void Apply(TicketDataUpdatedNotification notification)
    {
        BookingNumber = notification.BookingNumber;
        Name = notification.Name;

        var guidList = notification.SprintIds
            .Select(Guid.Parse)
            .ToList();

        SprintIds = new List<Guid>(guidList);
    }

    public void Apply(TicketDocumentationUpdatedNotification notification)
    {
        Documentation = notification.Documentation;
    }

    public void SynchronizeDocumentation(string documentation)
    {
        PreviousDocumentation = Documentation;
        Documentation = documentation;
    }

    public bool IsDocumentationChanged()
    {
        var result = !PreviousDocumentation.Equals(Documentation);
        PreviousDocumentation = Documentation;
        return result;
    }
}