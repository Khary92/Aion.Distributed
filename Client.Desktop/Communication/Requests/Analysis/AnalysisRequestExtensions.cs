using Client.Desktop.Communication.Requests.Analysis.Records;
using Proto.Requests.AnalysisData;

namespace Client.Desktop.Communication.Requests.Analysis;

public static class AnalysisRequestExtensions
{
    public static GetSprintAnalysisById ToProto(this ClientGetSprintAnalysisById request) => new()
    {
        SprintId = request.SprintId.ToString()
    };

    public static GetTicketAnalysisById ToProto(this ClientGetTicketAnalysisById request) => new()
    {
        TicketId = request.TicketId.ToString()
    };

    public static GetTagAnalysisById ToProto(this ClientGetTagAnalysisById request) => new()
    {
        TagId = request.TagId.ToString()
    };
}