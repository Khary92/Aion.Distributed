using Client.Desktop.Communication.Requests.Analysis.Records;
using Proto.Requests.AnalysisData;

namespace Client.Desktop.Communication.Requests.Analysis;

public static class AnalysisRequestExtensions
{
    public static GetSprintAnalysisById ToProto(this ClientGetSprintAnalysisById request)
    {
        return new GetSprintAnalysisById
        {
            SprintId = request.SprintId.ToString()
        };
    }

    public static GetTicketAnalysisById ToProto(this ClientGetTicketAnalysisById request)
    {
        return new GetTicketAnalysisById
        {
            TicketId = request.TicketId.ToString()
        };
    }

    public static GetTagAnalysisById ToProto(this ClientGetTagAnalysisById request)
    {
        return new GetTagAnalysisById
        {
            TagId = request.TagId.ToString()
        };
    }
}