using Service.Server.Communication.CQRS.Commands.Entities.Tags;

namespace Service.Server.Services.Entities.Tags;

public interface ITagCommandsService
{
    Task Update(UpdateTagCommand updateTagCommand);
    Task Create(CreateTagCommand createTagCommand);
}