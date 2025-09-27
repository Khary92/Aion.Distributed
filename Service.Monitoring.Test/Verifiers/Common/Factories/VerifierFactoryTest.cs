using Moq;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Verifiers.Common.Factories;

namespace Service.Monitoring.Test.Verifiers.Common.Factories;

[TestFixture]
[TestOf(typeof(VerifierFactory))]
public class VerifierFactoryTest
{
    [SetUp]
    public void Setup()
    {
        _mockReportFactory = new Mock<IReportFactory>();
        _verifierFactory = new VerifierFactory(_mockReportFactory.Object);
    }

    private Mock<IReportFactory> _mockReportFactory;
    private VerifierFactory _verifierFactory;

    [Test]
    public void Create_ShouldReturnVerifier_WithValidParameters()
    {
        var traceId = Guid.NewGuid();
        var sortingType = SortingType.Overview;
        var useCaseMeta = UseCaseMeta.CreateTicket;

        var verifier = _verifierFactory.Create(traceId, sortingType, useCaseMeta);

        Assert.That(verifier, Is.Not.Null);
    }

    [Test]
    public void Create_ShouldReturnVerifier_WithNonDefaultGuid()
    {
        var traceId = Guid.Empty;
        var sortingType = SortingType.Client;
        var useCaseMeta = UseCaseMeta.UpdateSprint;

        var verifier = _verifierFactory.Create(traceId, sortingType, useCaseMeta);

        Assert.That(verifier, Is.Not.Null);
    }

    [Test]
    public void Create_ShouldHandleDifferentUseCaseMetaValues()
    {
        var traceId = Guid.NewGuid();
        var sortingType = SortingType.TimerSettings;

        foreach (UseCaseMeta useCaseMeta in Enum.GetValues(typeof(UseCaseMeta)))
        {
            var verifier = _verifierFactory.Create(traceId, sortingType, useCaseMeta);

            Assert.That(verifier, Is.Not.Null);
        }
    }

    [Test]
    public void Create_ShouldHandleAllSortingTypeValues()
    {
        var traceId = Guid.NewGuid();
        var useCaseMeta = UseCaseMeta.CreateTrackingControl;

        foreach (SortingType sortingType in Enum.GetValues(typeof(SortingType)))
        {
            var verifier = _verifierFactory.Create(traceId, sortingType, useCaseMeta);

            Assert.That(verifier, Is.Not.Null);
        }
    }
}