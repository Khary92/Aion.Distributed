using Polly;

namespace Service.Monitoring.Communication;

public class TraceDataSendPolicy(IAsyncPolicy policy)
{
        public IAsyncPolicy Policy { get; } = policy;
}