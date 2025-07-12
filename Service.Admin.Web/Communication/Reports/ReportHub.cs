using Microsoft.AspNetCore.SignalR;

namespace Service.Admin.Web.Communication.Reports;

public class ReportHub : Hub
{
    public async Task SendMessage(ReportRecord report)
    {
        await Clients.All.SendAsync("ReceiveReport", report);
    }
}