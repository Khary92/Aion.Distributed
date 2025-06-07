using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Tags;

public record GetTagByIdRequest(Guid TagId) : IRequest<TagDto>, INotification;