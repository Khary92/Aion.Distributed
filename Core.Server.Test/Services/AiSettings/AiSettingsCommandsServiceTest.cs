using Core.Server.Communication.Endpoints.AiSettings;
using Core.Server.Communication.Records.Commands.Entities.AiSettings;
using Core.Server.Services.Entities.AiSettings;
using Core.Server.Translators.Commands.AiSettings;
using Domain.Events.AiSettings;
using Domain.Interfaces;
using Moq;

namespace Core.Server.Test.Services.AiSettings;

[TestFixture]
[TestOf(typeof(AiSettingsCommandsService))]
public class AiSettingsCommandsServiceTest
{
    [SetUp]
    public void SetUp()
    {
        _eventStoreMock = new Mock<IEventStore<AiSettingsEvent>>();
        _eventsTranslatorMock = new Mock<IAiSettingsCommandsToEventTranslator>();

        _instance = new AiSettingsCommandsService(_eventStoreMock.Object, _eventsTranslatorMock.Object,
            _notificationServiceMock.Object);
    }

    private Mock<IEventStore<AiSettingsEvent>> _eventStoreMock;
    private Mock<IAiSettingsCommandsToEventTranslator> _eventsTranslatorMock;
    private Mock<AiSettingsNotificationService> _notificationServiceMock;

    private AiSettingsCommandsService _instance;

    [Test]
    public async Task ChangeLanguageModelPath()
    {
        var command = new ChangeLanguageModelCommand(Guid.NewGuid(), "any");
        var domainEvent =
            new AiSettingsEvent(Guid.NewGuid(), DateTime.Now, TimeSpan.Zero, "any", Guid.NewGuid(), "Payload");

        _eventsTranslatorMock.Setup(x => x.ToEvent(command)).Returns(domainEvent);

        await _instance.ChangeLanguageModelPath(command);

        _eventsTranslatorMock.Verify(t => t.ToEvent(command), Times.Once);
        _eventStoreMock.Verify(es => es.StoreEventAsync(domainEvent), Times.Once);
    }
}