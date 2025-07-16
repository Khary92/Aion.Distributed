using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Service.Admin.Web.Communication.Reports.State;

namespace Service.Admin.Web.Communication.Reports.Hub;

public class ReportHubService(
    NavigationManager navigationManager,
    IConfiguration configuration,
    ILogger<ReportHubService> logger,
    IReportStateService stateService)
    : IReportHubService, IAsyncDisposable
{
    private readonly NavigationManager _navigationManager = navigationManager;
    private HubConnection? _hubConnection;
    
    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
    public event Action<ReportRecord>? OnReportReceived;

    public async Task InitializeAsync()
    {
        try
        {
            var hubUrl = configuration.GetValue<string>("HubSettings:ReportHubUrl") 
                         ?? "http://localhost:8080/reportHub";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<ReportRecord>("ReceiveReport", (record) =>
            {
                logger.LogInformation("Bericht empfangen: {TimeStamp}", record.TimeStamp);
                stateService.AddReport(record);
            });

            await _hubConnection.StartAsync();
            logger.LogInformation("Hub-Verbindung hergestellt");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fehler beim Initialisieren der Hub-Verbindung");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
            logger.LogInformation("Hub-Verbindung geschlossen");
        }
    }
}