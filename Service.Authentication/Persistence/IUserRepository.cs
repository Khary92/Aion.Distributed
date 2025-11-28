using Service.Authorization.Records;

namespace Service.Authorization.Persistence;

public interface IUserRepository
{
    List<User> GetAll();
}