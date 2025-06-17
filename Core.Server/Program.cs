using Service.Server;

var app = BootStrapper.BuildWebApp(args);
app.AddEndPoints();
app.AddMockingEndpoints();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
    foreach (var endpoint in endpointDataSource.Endpoints)
    {
        Console.WriteLine($"[Endpoint] {endpoint.DisplayName}");
    }
});

app.Run();