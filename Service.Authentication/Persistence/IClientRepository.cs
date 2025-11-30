using Service.Authorization.Records;

namespace Service.Authorization.Persistence;

public interface IClientRepository
{
    List<Client> GetAll();
}