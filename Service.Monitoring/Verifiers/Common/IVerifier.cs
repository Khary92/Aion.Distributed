using Service.Monitoring.Shared;
using Service.Monitoring.Verifiers.Common.Records;

namespace Service.Monitoring.Verifiers.Common;

public interface IVerifier
{
    event EventHandler<Report>? VerificationCompleted;
    void Add(TraceData traceData);
}