using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Server.Mock;

var builder = WebApplication.CreateBuilder(args);

// gRPC Services und Reflection registrieren
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
});

var app = builder.Build();

app.MapGrpcService<AiSettingsCommandServiceImpl>();
app.MapGrpcService<AiSettingsCommandServiceImpl>();

app.MapGrpcService<TicketNotificationServiceImpl>();
app.MapGrpcService<TicketCommandServiceImpl>();

// TODO remove in productive environment
app.MapGrpcReflectionService();

app.Run();