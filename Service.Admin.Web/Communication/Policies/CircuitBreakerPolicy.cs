using Polly;

namespace Service.Admin.Web.Communication.Policies;

public class CircuitBreakerPolicy(IAsyncPolicy policy)
{
    public IAsyncPolicy Policy { get; } = policy;
}
