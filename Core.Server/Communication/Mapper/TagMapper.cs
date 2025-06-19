using Proto.DTO.Tag;

namespace Service.Server.Communication.Mapper;

public class TagMapper : IDtoMapper<TagProto, Domain.Entities.Tag>
{
    public Domain.Entities.Tag ToDomain(TagProto dto)
    {
        return new Domain.Entities.Tag
        {
            TagId = Guid.Parse(dto.TagId),
            Name = dto.Name
        };
    }

    public TagProto ToDto(Domain.Entities.Tag domain)
    {
        return new TagProto
        {
            TagId = domain.TagId.ToString(),
            Name = domain.Name,
            IsSelected = false
        };
    }
}