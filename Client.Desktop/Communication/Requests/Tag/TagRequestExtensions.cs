using System;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.Notifications.Tag;
using Proto.Requests.Tags;

namespace Client.Desktop.Communication.Requests.Tag;

public static class TagRequestExtensions
{
    public static GetAllTagsRequestProto ToProto(this ClientGetAllTagsRequest request) => new();

    public static GetTagByIdRequestProto ToProto(this ClientGetTagByIdRequest request) => new()
    {
        TagId = request.TagId.ToString()
    };

    public static GetTagsByIdsRequestProto ToProto(this ClientGetTagsByIdsRequest request) => new()
    {
        TagIds = { request.TagIds.ToRepeatedField() }
    };
    
    public static TagClientModel ToWebModel(this TagCreatedNotification notification)
    {
        return new TagClientModel(Guid.Parse(notification.TagId), notification.Name, false);
    }
}