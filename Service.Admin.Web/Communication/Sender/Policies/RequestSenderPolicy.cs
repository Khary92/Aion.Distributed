using Polly;

namespace Service.Admin.Web.Communication.Sender.Policies;

public class RequestSenderPolicy(IAsyncPolicy policy)
{
    public IAsyncPolicy Policy { get; } = policy;
}