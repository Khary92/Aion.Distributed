using Service.Monitoring.Verifiers;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Communication;

public interface IReportSender
{
    Task Send(Report report);
}