using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Admin.Web;
using Service.Admin.Web.Communication.Reports;
using Service.Admin.Web.Communication.Tickets;
using Service.Admin.Web.Pages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebServices();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/.aspnet/DataProtection-Keys"));

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
app.UseResponseCompression();

app.MapHub<TicketHub>("/ticketHub");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
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

app.Run();