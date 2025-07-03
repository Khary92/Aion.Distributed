using Service.Monitoring.Verifiers;

namespace Service.Monitoring.Communication;

public interface IReportSender
{
    Task Send(Report report);
}