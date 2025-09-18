using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Analysis;
using CommunityToolkit.Mvvm.Messaging;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

[TestFixture]
[TestOf(typeof(AnalysisBySprintModel))]
public class AnalysisBySprintModelTest
{
    [Test]
    public async Task AddSprint()
    {
        var newSprintClientModel = new SprintClientModel(Guid.NewGuid(), "name", true, DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow, []);
        var newSprintMessage = new NewSprintMessage(newSprintClientModel, Guid.NewGuid());

        var fixture = await AnalysisBySprintModelProvider.Create(new List<SprintClientModel?>());
        fixture.Messenger.Send(newSprintMessage);

        Assert.That(fixture.Instance.Sprints, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ClientSprintDataUpdatedNotification()
    {
        var sprintId = Guid.NewGuid();
        var sprintName = "InitialSprintName";
        var changedSprintName = "ChangedSprintName";
        var newSprintClientModel = new SprintClientModel(sprintId, sprintName, true, DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow, new List<Guid>());

        var fixture =
            await AnalysisBySprintModelProvider.Create(new List<SprintClientModel?>() { newSprintClientModel });
        var clientSprintDataUpdateNotification = new ClientSprintDataUpdatedNotification(sprintId, changedSprintName,
            DateTimeOffset.MinValue, DateTimeOffset.MaxValue, Guid.NewGuid());

        fixture.Messenger.Send(clientSprintDataUpdateNotification);

        Assert.That(fixture.Instance.Sprints.First().Name, Is.EqualTo(changedSprintName));
    }

    [Test]
    public async Task GetMarkdownString()
    {
        var fixture = await AnalysisBySprintModelProvider.Create();

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
        "### Overview for InitialSprintName\r\n\r\n#### Top 3 most associated tags for Productive\r\n\r\n| Tag Name | Count |\r\n|----------|--------|\r\n| Productive Tag | 2 |\r\n\r\n\r\n#### Top 3 most associated tags for Neutral\r\n\r\n| Tag Name | Count |\r\n|----------|--------|\r\n| Neutral Tag | 1 |\r\n\r\n\r\n#### Top 3 most associated tags for Unproductive\r\n\r\n| Tag Name | Count |\r\n|----------|--------|\r\n| Unproductive Tag | 1 |\r\n\r\n";
}