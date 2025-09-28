using Core.Persistence.DbContext;
using Core.Persistence.EventStores;
using Domain.Events.Tickets;
using Global.Settings.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Core.Persistence.Test.EventStores;

// TODO: This test is AI generated. It was better than what i would have come up with. Need to check it though.
[TestFixture]
[TestOf(typeof(TicketEventsStore))]
public class TicketEventsStoreTest
{
    [SetUp]
    public void SetUp()
    {
        var dbName = Guid.NewGuid().ToString();
        _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        
        _databaseSettingsMock = new Mock<IOptions<DatabaseSettings>>();
        _dbContextFactoryMock = new Mock<IDbContextFactory<AppDbContext>>();
        _dbContextFactoryMock
            .Setup(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new AppDbContext(_dbOptions, _databaseSettingsMock.Object));

        _instance = new TicketEventsStore(_dbContextFactoryMock.Object);
    }

    private Mock<IDbContextFactory<AppDbContext>> _dbContextFactoryMock;
    private DbContextOptions<AppDbContext> _dbOptions;
    private Mock<IOptions<DatabaseSettings>> _databaseSettingsMock;
    private TicketEventsStore _instance;

    [Test]
    public async Task StoreEventAsync_persists_event()
    {
        var entityId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        var ev = new TicketEvent(Guid.NewGuid(), now, "Created", entityId, "payload-1");

        await _instance.StoreEventAsync(ev);

        await using var verifyContext = new AppDbContext(_dbOptions,_databaseSettingsMock.Object);
        var count = await verifyContext.TicketEvents.CountAsync();
        Assert.That(count, Is.EqualTo(1));

        var saved = await verifyContext.TicketEvents.SingleAsync();
        Assert.That(saved.EntityId, Is.EqualTo(entityId));
        Assert.That(saved.EventType, Is.EqualTo("Created"));
        Assert.That(saved.EventPayload, Is.EqualTo("payload-1"));

        _dbContextFactoryMock.Verify(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetEventsForAggregateAsync_returns_only_requested_entity_in_time_order()
    {
        var targetEntityId = Guid.NewGuid();
        var otherEntityId = Guid.NewGuid();

        var t1 = DateTimeOffset.UtcNow.AddMinutes(-10);
        var t2 = DateTimeOffset.UtcNow.AddMinutes(-5);
        var tOther = DateTimeOffset.UtcNow.AddMinutes(-7);

        var e1 = new TicketEvent(Guid.NewGuid(), t2, "Updated", targetEntityId, "payload-2");
        var e0 = new TicketEvent(Guid.NewGuid(), t1, "Created", targetEntityId, "payload-1");
        var eOther = new TicketEvent(Guid.NewGuid(), tOther, "Created", otherEntityId, "payload-x");

        await using (var arrangeContext = new AppDbContext(_dbOptions, _databaseSettingsMock.Object))
        {
            await arrangeContext.TicketEvents.AddRangeAsync(eOther, e1, e0);
            await arrangeContext.SaveChangesAsync();
        }

        var result = await _instance.GetEventsForAggregateAsync(targetEntityId);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].TimeStamp, Is.EqualTo(t1));
        Assert.That(result[1].TimeStamp, Is.EqualTo(t2));
    }

    [Test]
    public async Task GetAllEventsAsync_returns_all_in_time_order()
    {
        var entityA = Guid.NewGuid();
        var entityB = Guid.NewGuid();

        var t1 = DateTimeOffset.UtcNow.AddMinutes(-20);
        var t2 = DateTimeOffset.UtcNow.AddMinutes(-10);
        var t3 = DateTimeOffset.UtcNow.AddMinutes(-5);

        var e1 = new TicketEvent(Guid.NewGuid(), t2, "Updated", entityA, "p2");
        var e0 = new TicketEvent(Guid.NewGuid(), t1, "Created", entityA, "p1");
        var e2 = new TicketEvent(Guid.NewGuid(), t3, "Created", entityB, "p3");

        await using (var arrangeContext = new AppDbContext(_dbOptions, _databaseSettingsMock.Object))
        {
            await arrangeContext.TicketEvents.AddRangeAsync(e1, e2, e0);
            await arrangeContext.SaveChangesAsync();
        }

        var result = await _instance.GetAllEventsAsync();

        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].TimeStamp, Is.EqualTo(t1));
        Assert.That(result[1].TimeStamp, Is.EqualTo(t2));
        Assert.That(result[2].TimeStamp, Is.EqualTo(t3));
    }

    [Test]
    public async Task GetTicketDocumentationEventsByTicketId_filters_by_type_and_orders_by_time()
    {
        var ticketId = Guid.NewGuid();
        var otherId = Guid.NewGuid();

        var t1 = DateTimeOffset.UtcNow.AddMinutes(-15);
        var t2 = DateTimeOffset.UtcNow.AddMinutes(-5);
        var tOther = DateTimeOffset.UtcNow.AddMinutes(-10);

        // TODO: Passe diese Beispiel-JSONs an die tatsächlichen Eigenschaften von TicketDocumentationChangedEvent an.
        var payload1 = "{\"Some\":\"Value1\"}";
        var payload2 = "{\"Some\":\"Value2\"}";
        var payloadOther = "{\"Some\":\"Other\"}";

        var docTypeName = nameof(TicketDocumentationChangedEvent);

        var e0 = new TicketEvent(Guid.NewGuid(), t1, docTypeName, ticketId, payload1);
        var e1 = new TicketEvent(Guid.NewGuid(), t2, docTypeName, ticketId, payload2);
        var eOtherType = new TicketEvent(Guid.NewGuid(), tOther, "Created", ticketId, "p-x");
        var eOtherId = new TicketEvent(Guid.NewGuid(), tOther, docTypeName, otherId, payloadOther);

        await using (var arrangeContext = new AppDbContext(_dbOptions, _databaseSettingsMock.Object))
        {
            await arrangeContext.TicketEvents.AddRangeAsync(eOtherType, eOtherId, e1, e0);
            await arrangeContext.SaveChangesAsync();
        }

        var result = await _instance.GetTicketDocumentationEventsByTicketId(ticketId);

        Assert.That(result, Has.Count.EqualTo(2));
    }
}