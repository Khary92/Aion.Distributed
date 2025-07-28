using Client.Desktop.DataModels.Decorators;
using Proto.DTO.AnalysisBySprint;
using Proto.DTO.AnalysisByTag;
using Proto.DTO.AnalysisByTicket;

namespace Client.Desktop.Communication.Requests.Analysis;

public interface IAnalysisMapper
{
    AnalysisBySprintDecorator Create(AnalysisBySprintProto proto);
    AnalysisByTicketDecorator Create(AnalysisByTicketProto proto);
    AnalysisByTagDecorator Create(AnalysisByTagProto proto);
}