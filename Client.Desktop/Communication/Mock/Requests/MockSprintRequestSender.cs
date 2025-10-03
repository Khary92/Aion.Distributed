using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Google.Protobuf.WellKnownTypes;
using Proto.DTO.Sprint;
using Proto.Requests.Sprints;
using Service.Proto.Shared.Requests.Sprints;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockSprintRequestSender(MockDataService mockDataService) : ISprintRequestSender
{
    public Task<SprintProto?> Send(GetActiveSprintRequestProto request)
    {
        return Task.FromResult(ConvertToProto(mockDataService.Sprints.First()))!;
    }

    public Task<SprintListProto> Send(GetAllSprintsRequestProto request)
    {
        var result = new SprintListProto
        {
            Sprints = { mockDataService.Sprints.Select(ConvertToProto).ToList() }
        };

        return Task.FromResult(result);
    }

    private static SprintProto ConvertToProto(SprintClientModel sprintClientModel)
    {
        return new SprintProto
        {
            SprintId = sprintClientModel.SprintId.ToString(),
            TicketIds = { sprintClientModel.TicketIds.Select(id => id.ToString()).ToList() },
            Start = Timestamp.FromDateTimeOffset(sprintClientModel.StartTime),
            End = Timestamp.FromDateTimeOffset(sprintClientModel.EndTime),
            Name = sprintClientModel.Name,
            IsActive = sprintClientModel.IsActive
        };
    }
}