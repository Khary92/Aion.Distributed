using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Tags;

public record GetAllTagsRequest : IRequest<List<TagDto>>, INotification;