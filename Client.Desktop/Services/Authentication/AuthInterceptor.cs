using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Client.Desktop.Services.Authentication;

public class AuthInterceptor(Func<Task<string>> getToken) : Interceptor
{
    private async Task<Metadata> AddAuthorizationHeaderAsync(Metadata? headers)
    {
        headers ??= new Metadata();
        var token = await getToken();

        if (!string.IsNullOrWhiteSpace(token))
        {
            // Remove old header if it exists (e.g., after refresh)
            var existing = headers.FirstOrDefault(h => h.Key == "authorization");
            if (existing != null)
                headers.Remove(existing);

            headers.Add("authorization", $"Bearer {token}");
        }

        return headers;
    }

    // --------- Unary RPC ---------
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var newCtx = ContextWithAuthAsync(context).GetAwaiter().GetResult();
        return base.AsyncUnaryCall(request, newCtx, continuation);
    }

    // --------- Server Streaming ---------
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var newCtx = ContextWithAuthAsync(context).GetAwaiter().GetResult();
        return base.AsyncServerStreamingCall(request, newCtx, continuation);
    }

    // --------- Client Streaming ---------
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var newCtx = ContextWithAuthAsync(context).GetAwaiter().GetResult();
        return base.AsyncClientStreamingCall(newCtx, continuation);
    }

    // --------- Duplex Streaming ---------
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var newCtx = ContextWithAuthAsync(context).GetAwaiter().GetResult();
        return base.AsyncDuplexStreamingCall(newCtx, continuation);
    }

    // --------- Helper: build new context with JWT ---------
    private async Task<ClientInterceptorContext<TRequest, TResponse>> ContextWithAuthAsync<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        var headers = await AddAuthorizationHeaderAsync(context.Options.Headers);
        var options = context.Options.WithHeaders(headers);

        return new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
    }
}