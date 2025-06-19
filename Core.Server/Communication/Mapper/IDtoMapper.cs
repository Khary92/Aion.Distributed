namespace Service.Server.Communication.Mapper;

public interface IDtoMapper<TDtoType, TDomainType>
{
    TDomainType ToDomain(TDtoType dto);
    TDtoType ToDto(TDomainType domain);
}