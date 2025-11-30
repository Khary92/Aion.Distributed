using Service.Authorization.Records;

namespace Service.Authorization.Persistence;

public class UserRepository : IUserRepository
{
    public List<User> GetAll()
    {
        return [new User("alice", "password", "The thing"), new User("bob", "password", "The other alice")];
    }
}