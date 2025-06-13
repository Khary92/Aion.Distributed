using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Proto.Requests.Sprints;

public class SprintRequestServiceImpl : SprintRequestService.SprintRequestServiceBase
{
    public override Task<SprintProto> GetActiveSprint(GetActiveSprintRequestProto request, ServerCallContext context)
    {
        var sprint = new SprintProto
        {
            SprintId = "active-sprint-1",
            Name = "Sprint Alpha",
            Start = Timestamp.FromDateTime(System.DateTime.UtcNow.AddDays(-7).ToUniversalTime()),
            End = Timestamp.FromDateTime(System.DateTime.UtcNow.AddDays(7).ToUniversalTime()),
        };
        sprint.TicketIds.Add("ticket-1");
        sprint.TicketIds.Add("ticket-2");

        return Task.FromResult(sprint);
    }

    public override Task<SprintListProto> GetAllSprints(GetAllSprintsRequestProto request, ServerCallContext context)
    {
        var list = new SprintListProto();
        list.Sprints.Add(new SprintProto
        {
            SprintId = "sprint-1",
            Name = "Sprint Alpha",
            Start = Timestamp.FromDateTime(System.DateTime.UtcNow.AddDays(-30).ToUniversalTime()),
            End = Timestamp.FromDateTime(System.DateTime.UtcNow.AddDays(-15).ToUniversalTime())
        });
        list.Sprints.Add(new SprintProto
        {
            SprintId = "sprint-2",
            Name = "Sprint Beta",
            Start = Timestamp.FromDateTime(System.DateTime.UtcNow.AddDays(-14).ToUniversalTime()),
            End = Timestamp.FromDateTime(System.DateTime.UtcNow.AddDays(-1).ToUniversalTime())
        });

        return Task.FromResult(list);
    }
}