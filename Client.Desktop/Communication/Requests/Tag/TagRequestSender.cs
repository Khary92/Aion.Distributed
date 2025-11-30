using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Authentication;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Proto.Requests.Tags;

namespace Client.Desktop.Communication.Requests.Tag;

public class TagRequestSender(string address, ITokenService tokenService) : ITagRequestSender, IInitializeAsync
{
    private TagProtoRequestService.TagProtoRequestServiceClient? _client;

    public InitializationType Type => InitializationType.AuthToken;

    public async Task InitializeAsync()
    {
        var channel = GrpcChannel.ForAddress(address);
        var tokenProvider = new Func<Task<string>>(tokenService.GetToken);
        var callInvoker = channel.Intercept(new AuthInterceptor(tokenProvider));
        _client = new TagProtoRequestService.TagProtoRequestServiceClient(callInvoker);

        await Task.CompletedTask;
    }

    public async Task<List<TagClientModel>> Send(GetAllTagsRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TagRequestSender is not initialized");

        var allTagsAsync = await _client.GetAllTagsAsync(request);
        return allTagsAsync.ToClientModelList();
    }

    public async Task<TagClientModel> Send(GetTagByIdRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TagRequestSender is not initialized");

        var tagByIdAsync = await _client.GetTagByIdAsync(request);
        return tagByIdAsync.ToClientModel();
    }

    public async Task<List<TagClientModel>> Send(GetTagsByIdsRequestProto request)
    {
        if (_client is null) throw new InvalidOperationException("TagRequestSender is not initialized");

        var tagsByIdsAsync = await _client.GetTagsByIdsAsync(request);
        return tagsByIdsAsync.ToClientModelList();
    }
}