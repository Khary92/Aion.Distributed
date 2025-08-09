using Polly;

namespace Service.Admin.Web.Communication.Policies;

public class RequestSenderPolicy(IAsyncPolicy policy)
{
    public IAsyncPolicy Policy { get; } = policy;
}