using Global.Settings;
using Global.Settings.Types;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Admin.Tracing;
using Service.Admin.Web;
using Service.Admin.Web.Communication.Authentication;
using Service.Admin.Web.Communication.Receiver;
using Service.Admin.Web.Communication.Receiver.Reports;
using Service.Admin.Web.Pages;
using Service.Admin.Web.Services.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.SetConfiguration();
builder.Services.AddWebServices();
builder.Services.AddTracingServices();

builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true;
    options.Cookie.Name = "DevAntiforgeryDisabled";
});

var adminSettings = new AdminSettings();
builder.Configuration.GetSection("AdminSettings").Bind(adminSettings);

builder.WebHost.ConfigureKestrel(options =>
{
    // Internal gRPC Listener (HTTP/2, no TLS). Do not expose this outside of the docker network!
    options.ListenAnyIP(adminSettings.InternalGrpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
    });

    // Web Listener (HTTPS + HTTP/1/2)
    options.ListenAnyIP(adminSettings.ExposedWebPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

builder.Logging.AddConsole();

var app = builder.Build();

var jwtService = app.Services.GetRequiredService<JwtService>();
await jwtService.LoadTokenAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapGrpcService<TicketNotificationsReceiver>();
app.MapGrpcService<ReportReceiver>();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

var componentInitializer = app.Services.GetRequiredService<IComponentInitializer>();
await componentInitializer.InitializeServicesAsync();

app.Run();
