using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Client.Desktop.Services.Authentication;

public class AuthInterceptor : Interceptor
{
    private readonly ITokenService _tokenService;

    public AuthInterceptor(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        // Erstelle einen Task, der zuerst den Token holt und dann den echten Call startet
        var callTask = CreateCallWithTokenAsync(request, context, continuation);

        // Wrappe den echten Call in die Microsoft-Template-Struktur
        return new AsyncUnaryCall<TResponse>(
            HandleResponse(callTask),
            callTask.ContinueWith(t => t.Result.ResponseHeadersAsync).Unwrap(),
            () => callTask.Result.GetStatus(),
            () => callTask.Result.GetTrailers(),
            () => callTask.Result.Dispose()
        );
    }

    private async Task<AsyncUnaryCall<TResponse>> CreateCallWithTokenAsync<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        // asynchron den aktuellen Token holen
        var token = await _tokenService.GetToken();

        // Header vorbereiten
        var headers = context.Options.Headers ?? new Metadata();
        if (!string.IsNullOrWhiteSpace(token))
            headers.Add("Authorization", $"Bearer {token}");

        // neuen Kontext mit Header erstellen
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            context.Options.WithHeaders(headers)
        );

        // den echten RPC-Call starten
        return continuation(request, newContext);
    }

    private async Task<TResponse> HandleResponse<TResponse>(Task<AsyncUnaryCall<TResponse>> callTask)
        where TResponse : class
    {
        var call = await callTask;
        return await call.ResponseAsync;
    }
}