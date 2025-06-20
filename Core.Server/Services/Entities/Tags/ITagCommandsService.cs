using Service.Server.CQRS.Commands.Entities.Tags;

namespace Service.Server.Old.Services.Entities.Tags;

public interface ITagCommandsService
{
    Task Update(UpdateTagCommand updateTagCommand);
    Task Create(CreateTagCommand createTagCommand);
}