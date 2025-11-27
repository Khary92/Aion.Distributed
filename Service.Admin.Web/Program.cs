using System.Security.Cryptography.X509Certificates;
using Global.Settings;
using Global.Settings.Types;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Admin.Tracing;
using Service.Admin.Web;
using Service.Admin.Web.Communication.Receiver;
using Service.Admin.Web.Communication.Receiver.Reports;
using Service.Admin.Web.Pages;
using Service.Admin.Web.Services.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.SetConfiguration();
builder.Services.AddWebServices();
builder.Services.AddTracingServices();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtection-Keys"));

builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true;
    options.Cookie.Name = "DevAntiforgeryDisabled";
});

var adminSettings = new AdminSettings();
builder.Configuration.GetSection("AdminSettings").Bind(adminSettings);

builder.WebHost.ConfigureKestrel(options =>
{
    // Internal Grpc Listener (HTTP/2, no TLS)
    options.ListenAnyIP(adminSettings.InternalGrpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
    });
    
    // Web listener
    options.ListenAnyIP(adminSettings.ExposedWebPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

builder.Logging.AddConsole();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtection-Keys"))
    .SetApplicationName("Aion");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAntiforgery();

app.MapGrpcService<TicketNotificationsReceiver>();
app.MapGrpcService<ReportReceiver>();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

var componentInitializer = app.Services.GetRequiredService<IComponentInitializer>();
await componentInitializer.InitializeServicesAsync();

app.Run();