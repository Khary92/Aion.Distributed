using Polly;

namespace Client.Desktop.Communication.Policies;

public class CommandRetryPolicy(IAsyncPolicy policy)
{
    public IAsyncPolicy Policy { get; } = policy;
}