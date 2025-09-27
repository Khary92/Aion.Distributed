using Core.Server.Communication.Endpoints.Analysis;
using Core.Server.Services.Client;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Services.Entities.Tags;
using Core.Server.Services.Entities.Tickets;
using Grpc.Core;
using Moq;
using Proto.DTO.AnalysisBySprint;
using Proto.DTO.AnalysisByTag;
using Proto.DTO.AnalysisByTicket;
using Proto.Requests.AnalysisData;

namespace Core.Server.Test.Communication.Endpoints.Analysis;

[TestFixture]
[TestOf(typeof(AnalysisRequestReceiver))]
public class AnalysisRequestReceiverTest
{
    [SetUp]
    public void SetUp()
    {
        _analysisDataServiceMock = new Mock<IAnalysisDataService>();
        _sprintRequestsServiceMock = new Mock<ISprintRequestsService>();
        _tagRequestsServiceMock = new Mock<ITagRequestsService>();
        _ticketRequestsServiceMock = new Mock<ITicketRequestsService>();

        _receiver = new AnalysisRequestReceiver(
            _analysisDataServiceMock.Object,
            _sprintRequestsServiceMock.Object,
            _tagRequestsServiceMock.Object,
            _ticketRequestsServiceMock.Object
        );
    }

    private Mock<IAnalysisDataService> _analysisDataServiceMock;
    private Mock<ISprintRequestsService> _sprintRequestsServiceMock;
    private Mock<ITagRequestsService> _tagRequestsServiceMock;
    private Mock<ITicketRequestsService> _ticketRequestsServiceMock;

    private AnalysisRequestReceiver _receiver;

    [Test]
    public async Task GetSprintAnalysis_ValidSprintId_ReturnsAnalysisBySprintProto()
    {
        var sprintId = Guid.NewGuid();
        var request = new GetSprintAnalysisById { SprintId = sprintId.ToString() };
        var expectedAnalysis = new AnalysisBySprintProto();
        var sprint = new Domain.Entities.Sprint();

        _sprintRequestsServiceMock
            .Setup(service => service.GetById(sprintId))
            .ReturnsAsync(sprint);

        _analysisDataServiceMock
            .Setup(service => service.GetAnalysisBySprint(sprint))
            .ReturnsAsync(expectedAnalysis);

        var result = await _receiver.GetSprintAnalysis(request, It.IsAny<ServerCallContext>());

        Assert.That(expectedAnalysis, Is.EqualTo(result));
        _sprintRequestsServiceMock.Verify(service => service.GetById(sprintId), Times.Once);
        _analysisDataServiceMock.Verify(service => service.GetAnalysisBySprint(sprint), Times.Once);
    }

    [Test]
    public void GetSprintAnalysis_InvalidSprintId_ThrowsFormatException()
    {
        var request = new GetSprintAnalysisById { SprintId = "InvalidGuid" };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _receiver.GetSprintAnalysis(request, It.IsAny<ServerCallContext>())
        );

        _sprintRequestsServiceMock.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Never);
        _analysisDataServiceMock.Verify(service => service.GetAnalysisBySprint(It.IsAny<Domain.Entities.Sprint>()),
            Times.Never);
    }

    [Test]
    public async Task GetTagAnalysis_ValidTagId_ReturnsAnalysisByTagProto()
    {
        var tagId = Guid.NewGuid();
        var request = new GetTagAnalysisById { TagId = tagId.ToString() };
        var expectedAnalysis = new AnalysisByTagProto();
        var tag = new Domain.Entities.Tag();

        _tagRequestsServiceMock
            .Setup(service => service.GetTagById(tagId))
            .ReturnsAsync(tag);

        _analysisDataServiceMock
            .Setup(service => service.GetAnalysisByTag(tag))
            .ReturnsAsync(expectedAnalysis);

        var result = await _receiver.GetTagAnalysis(request, It.IsAny<ServerCallContext>());

        Assert.That(expectedAnalysis, Is.EqualTo(result));
        _tagRequestsServiceMock.Verify(service => service.GetTagById(tagId), Times.Once);
        _analysisDataServiceMock.Verify(service => service.GetAnalysisByTag(tag), Times.Once);
    }

    [Test]
    public void GetTagAnalysis_InvalidTagId_ThrowsFormatException()
    {
        var request = new GetTagAnalysisById { TagId = "InvalidGuid" };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _receiver.GetTagAnalysis(request, It.IsAny<ServerCallContext>())
        );

        _tagRequestsServiceMock.Verify(service => service.GetTagById(It.IsAny<Guid>()), Times.Never);
        _analysisDataServiceMock.Verify(service => service.GetAnalysisByTag(It.IsAny<Domain.Entities.Tag>()),
            Times.Never);
    }

    [Test]
    public async Task GetTicketAnalysis_ValidTicketId_ReturnsAnalysisByTicketProto()
    {
        var ticketId = Guid.NewGuid();
        var request = new GetTicketAnalysisById { TicketId = ticketId.ToString() };
        var expectedAnalysis = new AnalysisByTicketProto();
        var ticket = new Domain.Entities.Ticket();

        _ticketRequestsServiceMock
            .Setup(service => service.GetTicketById(ticketId))
            .ReturnsAsync(ticket);

        _analysisDataServiceMock
            .Setup(service => service.GetAnalysisByTicket(ticket))
            .ReturnsAsync(expectedAnalysis);

        var result = await _receiver.GetTicketAnalysis(request, It.IsAny<ServerCallContext>());

        Assert.That(expectedAnalysis, Is.EqualTo(result));
        _ticketRequestsServiceMock.Verify(service => service.GetTicketById(ticketId), Times.Once);
        _analysisDataServiceMock.Verify(service => service.GetAnalysisByTicket(ticket), Times.Once);
    }

    [Test]
    public void GetTicketAnalysis_InvalidTicketId_ThrowsFormatException()
    {
        var request = new GetTicketAnalysisById { TicketId = "InvalidGuid" };

        Assert.ThrowsAsync<FormatException>(async () =>
            await _receiver.GetTicketAnalysis(request, It.IsAny<ServerCallContext>())
        );

        _ticketRequestsServiceMock.Verify(service => service.GetTicketById(It.IsAny<Guid>()), Times.Never);
        _analysisDataServiceMock.Verify(service => service.GetAnalysisByTicket(It.IsAny<Domain.Entities.Ticket>()),
            Times.Never);
    }
}