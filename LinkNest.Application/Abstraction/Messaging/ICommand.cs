using LinkNest.Domain.Abstraction;
using MediatR;

namespace LinkNest.Application.Abstraction.Messaging
{
        public interface ICommand : IRequest<Result>
        {

        }

        public interface ICommand<TResponse> : IRequest<Result<TResponse>>
        {
        }

        public interface IAuthCommand : IRequest<AuthResult>
        {
        }

}
