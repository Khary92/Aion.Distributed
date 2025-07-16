namespace Service.Admin.Web.Communication.Reports;

public interface IReportHubService
{
    event Action<ReportRecord>? OnReportReceived;
    bool IsConnected { get; }
    Task InitializeAsync();
    ValueTask DisposeAsync();
}