using Grpc.Core;
using Proto.Command.Sprints;
using Proto.Notifications.Sprint;

namespace Service.Server.Mock.Sprint;

public class SprintCommandServiceImpl(SprintNotificationServiceImpl sprintsNotificationService)
    : SprintsCommandService.SprintsCommandServiceBase
{
    public override async Task<CommandResponse> AddTicketToActiveSprint(AddTicketToActiveSprintCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[AddTicketToActiveSprint] TicketID: {request.TicketId}");

        try
        {
            await sprintsNotificationService.SendNotificationAsync(new SprintNotification
            {
                TicketAddedToActiveSprint = new TicketAddedToActiveSprintNotification
                {
                    TicketId = request.TicketId
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] AddTicketToActiveSprint failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> AddTicketToSprint(AddTicketToSprintCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[AddTicketToSprint] SprintID: {request.SprintId}, TicketID: {request.TicketId}");

        try
        {
            await sprintsNotificationService.SendNotificationAsync(new SprintNotification
            {
                TicketAddedToSprint = new TicketAddedToSprintNotification
                {
                    SprintId = request.SprintId,
                    TicketId = request.TicketId
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] AddTicketToSprint failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> CreateSprint(CreateSprintCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[CreateSprint] SprintID: {request.SprintId}, Name: {request.Name}, Start: {request.StartTime}, End: {request.EndTime}, IsActive: {request.IsActive}, Tickets: {string.Join(", ", request.TicketIds)}");

        try
        {
            await sprintsNotificationService.SendNotificationAsync(new SprintNotification
            {
                SprintCreated = new SprintCreatedNotification
                {
                    SprintId = request.SprintId,
                    Name = request.Name,
                    StartTime = request.StartTime ,
                    EndTime = request.EndTime,
                    IsActive = request.IsActive,
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] CreateSprint failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> SetSprintActiveStatus(SetSprintActiveStatusCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[SetSprintActiveStatus] SprintID: {request.SprintId}, IsActive: {request.IsActive}");

        try
        {
            await sprintsNotificationService.SendNotificationAsync(new SprintNotification
            {
                SprintActiveStatusSet = new SprintActiveStatusSetNotification
                {
                    SprintId = request.SprintId,
                    IsActive = request.IsActive,
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] SetSprintActiveStatus failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }

    public override async Task<CommandResponse> UpdateSprintData(UpdateSprintDataCommand request, ServerCallContext context)
    {
        Console.WriteLine($"[UpdateSprintData] SprintID: {request.SprintId}, Name: {request.Name}, Start: {request.StartTime}, End: {request.EndTime}");

        try
        {
            await sprintsNotificationService.SendNotificationAsync(new SprintNotification
            {
                SprintDataUpdated = new SprintDataUpdatedNotification
                {
                    SprintId = request.SprintId,
                    Name = request.Name,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                }
            });

            return new CommandResponse { Success = true };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Error] UpdateSprintData failed: {ex.Message}");
            return new CommandResponse { Success = false };
        }
    }
}
