using Polly;

namespace Service.Admin.Web.Communication.Policies;

public class RetryPolicy(IAsyncPolicy policy)
{
    public IAsyncPolicy Policy { get; } = policy;
}
