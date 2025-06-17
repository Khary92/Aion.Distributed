using Application.Contract.CQRS.Commands.Entities.Tags;

namespace Application.Services.Entities.Tags;

public interface ITagCommandsService
{
    Task Update(UpdateTagCommand updateTagCommand);
    Task Create(CreateTagCommand createTagCommand);
}