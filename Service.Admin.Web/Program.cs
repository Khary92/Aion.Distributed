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

var globalSettings = new GlobalSettings();
builder.Configuration.GetSection("GlobalSettings").Bind(globalSettings);

var adminSettings = new AdminSettings();
builder.Configuration.GetSection("ServerSettings").Bind(adminSettings);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(adminSettings.Port, listenOptions =>
    {
        if (globalSettings.UseHttps)
        {
            listenOptions.UseHttps("/app/certs/server.pfx");
        }

        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Logging.AddConsole();

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