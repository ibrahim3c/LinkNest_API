using LinkNest.Domain.Abstraction;
using MediatR;
using static LinkNest.Application.Abstraction.Messaging.ICommand;

namespace LinkNest.Application.Abstraction.Messaging
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {

    }

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {

    }


    public interface IAuthCommandHandler<in TCommand> : IRequestHandler<TCommand, AuthResult>
    where TCommand : IAuthCommand
    {

    }
}
