namespace Application.Mapper;

public interface IDtoMapper<TDtoType, TDomainType>
{
    TDomainType ToDomain(TDtoType dto);
    TDtoType ToDto(TDomainType domain);
}