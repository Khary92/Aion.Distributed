using Service.Server;

var builder = WebApplication.CreateBuilder(args);

// gRPC Service registrieren
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "gRPC Server läuft!");

app.Run();