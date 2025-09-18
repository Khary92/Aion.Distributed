using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Analysis;
using CommunityToolkit.Mvvm.Messaging;

namespace Client.Desktop.Test.Presentation.Models.Analysis;

[TestFixture]
[TestOf(typeof(AnalysisByTagModel))]
public class AnalysisByTagModelTest
{
    [Test]
    public async Task AddTag()
    {
        var newTagModel = new TagClientModel(Guid.NewGuid(), "name", true);
        var newTagMessage = new NewTagMessage(newTagModel, Guid.NewGuid());

        var fixture = await AnalysisByTagModelProvider.Create(new List<TagClientModel>());
        fixture.Messenger.Send(newTagMessage);

        Assert.That(fixture.Instance.Tags, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ClientTagDataUpdatedNotification()
    {
        var tagId = Guid.NewGuid();
        var tagName = "InitialTagName";
        var changedTagName = "ChangedTagName";
        var newTagClientModel = new TagClientModel(tagId, tagName, true);

        var fixture =
            await AnalysisByTagModelProvider.Create(new List<TagClientModel>() { newTagClientModel });
        var clientTagDataUpdateNotification = new ClientTagUpdatedNotification(tagId, changedTagName, Guid.NewGuid());

        fixture.Messenger.Send(clientTagDataUpdateNotification);

        Assert.That(fixture.Instance.Tags.First().Name, Is.EqualTo(changedTagName));
    }
}