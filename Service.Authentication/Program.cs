using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Service.Authorization;
using Service.Authorization.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting();
builder.Services.AddEndpointsApiExplorer();
    
var keyPath = "/certs/private_key.pem";

var rsa = RSA.Create();
rsa.ImportFromPem(File.ReadAllText(keyPath));

var rsaKey = new RsaSecurityKey(rsa)
{
    KeyId = "auth-server-key"
};

builder.Services.AddAuthServices(rsaKey);

var app = builder.Build();

app.MapGet("/authorize", req => app.Services.GetRequiredService<AuthoritaionEndpoint>().Handle(req.Request, req.Response));
app.MapPost("/token", req => app.Services.GetRequiredService<TokenEndpoint>().Handle(req.Request));
app.MapGet("/userinfo", req => app.Services.GetRequiredService<UserInfoEndpoint>().Handle(req.Request));

app.Run("http://0.0.0.0:5001");