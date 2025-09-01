using Grpc.Core;
using Proto.Command.Tags;
using Proto.Notifications.Tag;
using TagNotificationService = Core.Server.Communication.Endpoints.Tag.TagNotificationService;

namespace Core.Server.Communication.Mocks.Tag;

public class MockTagCommandService(TagNotificationService tagNotificationService)
    : TagCommandProtoService.TagCommandProtoServiceBase
{
    public override async Task<CommandResponse> CreateTag(CreateTagCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateTag] ID: {request.TagId}, Name: {request.Name}");

        try
        {
            await tagNotificationService.SendNotificationAsync(new TagNotification
            {
                TagCreated = new TagCreatedNotification
                {
                    TagId = request.TagId,
                    Name = request.Name
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[Error] CreateTag failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> UpdateTag(UpdateTagCommandProto request, ServerCallContext context)
    {
        Console.WriteLine($"[UpdateTag] ID: {request.TagId}, Name: {request.Name}");

        try
        {
            await tagNotificationService.SendNotificationAsync(new TagNotification
            {
                TagUpdated = new TagUpdatedNotification
                {
                    TagId = request.TagId,
                    Name = request.Name
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"[Error] UpdateTag failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}