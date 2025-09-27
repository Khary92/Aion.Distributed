using Polly;

namespace Service.Admin.Web.Communication.Sender.Policies;

public class CommandSenderPolicy(IAsyncPolicy policy)
{
    public IAsyncPolicy Policy { get; } = policy;
}