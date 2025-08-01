using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Admin.Tracing;
using Service.Admin.Web;
using Service.Admin.Web.Communication.Reports;
using Service.Admin.Web.Communication.Tickets;
using Service.Admin.Web.Pages;
using Service.Admin.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebServices();
builder.Services.AddTracingServices();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/DataProtection-Keys"));

builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true;
    options.Cookie.Name = "DevAntiforgeryDisabled";
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, o => { o.Protocols = HttpProtocols.Http1AndHttp2; });
    options.ListenAnyIP(8081, o => { o.Protocols = HttpProtocols.Http2; });
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
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var componentInitializer = app.Services.GetRequiredService<IComponentInitializer>();
await componentInitializer.InitializeServicesAsync(); 

app.Run();