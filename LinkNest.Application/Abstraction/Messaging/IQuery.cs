using LinkNest.Domain.Abstraction;
using MediatR;

namespace LinkNest.Application.Abstraction.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {

    }
}
