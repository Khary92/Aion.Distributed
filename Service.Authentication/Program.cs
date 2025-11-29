using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Service.Authorization;
using Service.Authorization.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting();
builder.Services.AddEndpointsApiExplorer();

var keyPath = "/certs/private_key_pkcs8.pem";
var pubKeyPath = "/certs/public_key.pem";

//Local debug
//var keyPath = "./private_key_pkcs8.pem";
//var pubKeyPath = "./public_key.pem";


RSA rsa;
if (!File.Exists(keyPath))
{
    rsa = RSA.Create(2048);

    var privPem = rsa.ExportPkcs8PrivateKeyPem();
    Directory.CreateDirectory(Path.GetDirectoryName(keyPath)!);
    File.WriteAllText(keyPath, privPem);

    var pubPem = rsa.ExportSubjectPublicKeyInfoPem();
    File.WriteAllText(pubKeyPath, pubPem);
}
else
{
    rsa = RSA.Create();
    rsa.ImportFromPem(File.ReadAllText(keyPath));
}

var rsaKey = new RsaSecurityKey(rsa)
{
    KeyId = "auth-server-key"
};

builder.Services.AddAuthServices(rsaKey);

var app = builder.Build();

app.MapGet("/authorize", async context =>
{
    var endpoint = context.RequestServices.GetRequiredService<AuthorizationEndpoint>();
    var result = await endpoint.Handle(context);
    await result.ExecuteAsync(context);
});

app.MapPost("/token", async context =>
{
    var endpoint = context.RequestServices.GetRequiredService<TokenEndpoint>();
    var result = await endpoint.Handle(context);
    await result.ExecuteAsync(context);
});

app.MapGet("/userinfo", async context =>
{
    var endpoint = context.RequestServices.GetRequiredService<UserInfoEndpoint>();
    await endpoint.Handle(context);
});

app.Run("http://0.0.0.0:5001");