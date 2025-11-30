using Service.Authorization.Records;

namespace Service.Authorization.Persistence;

public class ClientRepository : IClientRepository
{
    public List<Client> GetAll()
    {
        return
        [
            new Client("demo_client", ["http://localhost:5002/callback"], true,
                ["openid", "profile", "api"])
        ];
    }
}