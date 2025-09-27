using Core.Persistence.DbContext;
using Core.Persistence.EventStores;
using Domain.Events.NoteTypes;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Core.Persistence.Test.EventStores;

// TODO: This test is AI generated. It was better than what i would have come up with. Need to check it though.
[TestFixture]
[TestOf(typeof(NoteTypeEventsStore))]
public class NoteTypeEventsStoreTest
{
    [SetUp]
    public void SetUp()
    {
        var dbName = Guid.NewGuid().ToString();
        _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        _dbContextFactoryMock = new Mock<IDbContextFactory<AppDbContext>>();
        _dbContextFactoryMock
            .Setup(factory => factory.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new AppDbContext(_dbOptions));

        _instance = new NoteTypeEventsStore(_dbContextFactoryMock.Object);
    }

    private Mock<IDbContextFactory<AppDbContext>> _dbContextFactoryMock;
    private DbContextOptions<AppDbContext> _dbOptions;
    private NoteTypeEventsStore _instance;

    [Test]
    public async Task StoreEventAsync_persists_event()
    {
        var entityId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        var ev = new NoteTypeEvent(Guid.NewGuid(), now, "Created", entityId, "payload-1");

        await _instance.StoreEventAsync(ev);

        await using var verifyContext = new AppDbContext(_dbOptions);
        var count = await verifyContext.NoteTypeEvents.CountAsync();
        Assert.That(count, Is.EqualTo(1));

        var saved = await verifyContext.NoteTypeEvents.SingleAsync();
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

        var e1 = new NoteTypeEvent(Guid.NewGuid(), t2, "Updated", targetEntityId, "payload-2");
        var e0 = new NoteTypeEvent(Guid.NewGuid(), t1, "Created", targetEntityId, "payload-1");
        var eOther = new NoteTypeEvent(Guid.NewGuid(), tOther, "Created", otherEntityId, "payload-x");

        await using (var arrangeContext = new AppDbContext(_dbOptions))
        {
            await arrangeContext.NoteTypeEvents.AddRangeAsync(eOther, e1, e0);
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

        var e1 = new NoteTypeEvent(Guid.NewGuid(), t2, "Updated", entityA, "p2");
        var e0 = new NoteTypeEvent(Guid.NewGuid(), t1, "Created", entityA, "p1");
        var e2 = new NoteTypeEvent(Guid.NewGuid(), t3, "Created", entityB, "p3");

        await using (var arrangeContext = new AppDbContext(_dbOptions))
        {
            await arrangeContext.NoteTypeEvents.AddRangeAsync(e1, e2, e0);
            await arrangeContext.SaveChangesAsync();
        }

        var result = await _instance.GetAllEventsAsync();

        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].TimeStamp, Is.EqualTo(t1));
        Assert.That(result[1].TimeStamp, Is.EqualTo(t2));
        Assert.That(result[2].TimeStamp, Is.EqualTo(t3));
    }
}