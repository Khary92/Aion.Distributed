using System;
using System.Threading.Tasks;
using Grpc.Core;
using Proto.Command.Tags;
using Proto.Notifications.Tags;

namespace Service.Server.Mock;

public class TagCommandServiceImpl(TagNotificationServiceImpl tagNotificationService)
    : TagCommandService.TagCommandServiceBase
{
    public override async Task<CommandResponse> CreateTag(CreateTagCommand request, ServerCallContext context)
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
            Console.Error.WriteLine($"[Error] CreateTag failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> UpdateTag(UpdateTagCommand request, ServerCallContext context)
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
            Console.Error.WriteLine($"[Error] UpdateTag failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}