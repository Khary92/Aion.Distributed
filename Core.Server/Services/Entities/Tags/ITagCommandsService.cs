using Core.Server.Communication.Records.Commands.Entities.Tags;

namespace Core.Server.Services.Entities.Tags;

public interface ITagCommandsService
{
    Task Update(UpdateTagCommand updateTagCommand);
    Task Create(CreateTagCommand createTagCommand);
}