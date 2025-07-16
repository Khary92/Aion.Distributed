using Microsoft.AspNetCore.SignalR;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Sprints;

public class SprintHub : Hub
{
    public static class Notifications
    {
        public const string SprintCreated = nameof(ReceiveSprintCreated);
        public const string SprintDataUpdated = nameof(ReceiveSprintDataUpdated);
        public const string SprintActiveStatusSet = nameof(ReceiveSprintActiveStatusSet);
        public const string AddTicketToActiveSprint = nameof(AddTicketToActiveSprint);
        public const string AddTicketToSprint = nameof(AddTicketToSprint);
    }

    public async Task ReceiveSprintCreated(SprintDto sprint)
        => await Clients.All.SendAsync(Notifications.SprintCreated, sprint);

    public async Task ReceiveSprintDataUpdated(WebSprintDataUpdatedNotification command)
        => await Clients.All.SendAsync(Notifications.SprintDataUpdated, command);

    public async Task ReceiveSprintActiveStatusSet(WebSetSprintActiveStatusNotification command)
        => await Clients.All.SendAsync(Notifications.SprintActiveStatusSet, command);
    
    public async Task ReceiveTicketAddedToActiveSprint(WebAddTicketToActiveSprintNotification command)
        => await Clients.All.SendAsync(Notifications.SprintActiveStatusSet, command);
    
    public async Task ReceiveTicketAddedToSprint(WebAddTicketToSprintNotification command)
        => await Clients.All.SendAsync(Notifications.SprintActiveStatusSet, command);
}