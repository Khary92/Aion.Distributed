using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Analysis;
using CommunityToolkit.Mvvm.Messaging;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

[TestFixture]
[TestOf(typeof(AnalysisByTicketModel))]
public class AnalysisByTicketModelTest
{
    [Test]
    public async Task AddTicket()
    {
        var newTicketClientModel = new TicketClientModel(Guid.NewGuid(), "name", "bookingNumber", "documentation", []);
        var newTicketMessage = new NewTicketMessage(newTicketClientModel, Guid.NewGuid());

        var fixture = await AnalysisByTicketModelProvider.Create([]);
        fixture.Messenger.Send(newTicketMessage);

        Assert.That(fixture.Instance.Tickets, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ClientTicketDataUpdatedNotification()
    {
        var ticketId = Guid.NewGuid();
        var ticketName = "InitialTicketName";
        var changedTicketName = "ChangedTicketName";
        var newTicketClientModel = new TicketClientModel(ticketId, ticketName, "bookingNumber", "Documentation", []);

        var fixture =
            await AnalysisByTicketModelProvider.Create(new List<TicketClientModel?>() { newTicketClientModel });
        var clientTicketDataUpdateNotification = new ClientTicketDataUpdatedNotification(ticketId, changedTicketName,
            "BookingNumber", [], Guid.NewGuid());

        fixture.Messenger.Send(clientTicketDataUpdateNotification);

        Assert.That(fixture.Instance.Tickets.First().Name, Is.EqualTo(changedTicketName));
    }

    [Test]
    public async Task GetMarkdownString()
    {
        var fixture = await AnalysisByTicketModelProvider.Create();

        var markdownString = fixture.Instance.GetMarkdownString();

        var normalizedActual = NormalizeNewLinesToLf(markdownString);
        var normalizedExpected = NormalizeNewLinesToLf(ExpectedMarkdownString);

        Assert.That(normalizedActual, Is.EqualTo(normalizedExpected));
    }

    private static string NormalizeNewLinesToLf(string input)
    {
        return input.Replace("\r\n", "\n").Replace("\r", "\n");
    }

    private static string ExpectedMarkdownString =>
        "### Overview for TicketName\n\n#### Top 3 most associated tags for Productive\n\n| Tag Name | Count |\n|----------|--------|\n| Productive Tag | 2 |\n\n\n#### Top 3 most associated tags for Neutral\n\n| Tag Name | Count |\n|----------|--------|\n| Neutral Tag | 1 |\n\n\n#### Top 3 most associated tags for Unproductive\n\n| Tag Name | Count |\n|----------|--------|\n| Unproductive Tag | 1 |\n\n";
}