using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class TagMapper : IDtoMapper<TagDto, Tag>
{
    public Tag ToDomain(TagDto dto)
    {
        return new Tag
        {
            TagId = dto.TagId,
            Name = dto.Name
        };
    }

    public TagDto ToDto(Tag domain)
    {
        return new TagDto(domain.TagId, domain.Name, false);
    }
}