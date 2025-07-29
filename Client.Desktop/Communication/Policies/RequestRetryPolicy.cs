using Polly;

namespace Client.Desktop.Communication.Policies;

public class RequestRetryPolicy(IAsyncPolicy policy)
{
    public IAsyncPolicy Policy { get; } = policy;
}