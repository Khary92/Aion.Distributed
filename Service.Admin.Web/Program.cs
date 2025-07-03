using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Admin.Web.Communication;
using Service.Admin.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add gRPC services
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
    options.MaxSendMessageSize = 2 * 1024 * 1024; // 2 MB
});

// Register ReportReceiver as singleton to ensure same instance is used everywhere
builder.Services.AddSingleton<ReportEventHandler>();
builder.Services.AddSingleton<IReportEventHandler>(sp => sp.GetRequiredService<ReportEventHandler>());

// Register ReportReceiver as singleton and as gRPC service
builder.Services.AddSingleton<ReportReceiver>();
builder.Services.AddSingleton<IReportReceiver>(sp => sp.GetRequiredService<ReportReceiver>());

builder.Services.AddSingleton<ReportEventBridge>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, o => 
    { 
        o.Protocols = HttpProtocols.Http1AndHttp2;
    });
    
    // Port 5001 nur für gRPC (HTTP/2)
    options.ListenAnyIP(8081, o => 
    { 
        o.Protocols = HttpProtocols.Http2;
    });
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapGrpcService<ReportReceiver>();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

builder.Logging.AddConsole();

app.Run();