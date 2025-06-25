using Domain.Entities;
using Domain.Events.Settings;

namespace Core.Domain.Test.Entities;

[TestFixture]
[TestOf(typeof(Settings))]
public class SettingsTest : SettingsTestBase
{
    private static readonly Guid InitialId = Guid.NewGuid();
    private const string InitialExportPath = "Export A";
    private const bool InitialAddToSprint = true;

    [Test]
    public void CreatedEventSetsInitialState()
    {
        var created = new SettingsCreatedEvent(InitialId, InitialExportPath, InitialAddToSprint);
        var events = CreateEventList(created);
        var aggregate = Rehydrate(events);
        AssertSettingsState(aggregate, InitialId, InitialExportPath, InitialAddToSprint);
    }
    
    [Test]
    public void ExportPathChangedEventChangesFields()
    {
        const string newPath = "New Path";

        var created = new SettingsCreatedEvent(InitialId, InitialExportPath, InitialAddToSprint);
        var updated = new ExportPathChangedEvent(InitialId, newPath);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertSettingsState(aggregate, InitialId, newPath, InitialAddToSprint);
    }
    
    [Test]
    public void AutomaticAddingTicketToSprintChangedEventChangesFields()
    {
        const bool newIsAddNewTicketsToCurrentSprintActive = false;

        var created = new SettingsCreatedEvent(InitialId, InitialExportPath, InitialAddToSprint);
        var updated = new AutomaticAddingTicketToSprintChangedEvent(InitialId, newIsAddNewTicketsToCurrentSprintActive);
        var events = CreateEventList(created, updated);

        var aggregate = Rehydrate(events);

        AssertSettingsState(aggregate, InitialId, InitialExportPath, newIsAddNewTicketsToCurrentSprintActive);
    }
}