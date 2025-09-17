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
            DateTimeOffset.UtcNow, new List<Guid>());
        var newSprintMessage = new NewSprintMessage(newSprintClientModel, Guid.NewGuid());

        var fixture = await ModelTestProvider.CreateAnalysisBySprintModelAsync(new List<SprintClientModel?>());
        fixture.Messenger.Send(newSprintMessage);

        Assert.That(fixture.Instance.Sprints.Count, Is.EqualTo(1));
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
            await ModelTestProvider.CreateAnalysisBySprintModelAsync(new List<SprintClientModel?>()
                { newSprintClientModel });

        var clientSprintDataUpdateNotification = new ClientSprintDataUpdatedNotification(sprintId, changedSprintName,
            DateTimeOffset.MinValue, DateTimeOffset.MaxValue, Guid.NewGuid());
        
        fixture.Messenger.Send(clientSprintDataUpdateNotification);
        
        Assert.That(fixture.Instance.Sprints.First().Name, Is.EqualTo(changedSprintName));
    }
}