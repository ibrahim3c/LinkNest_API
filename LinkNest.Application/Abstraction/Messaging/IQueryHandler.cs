﻿using LinkNest.Domain.Abstraction;
using MediatR;

namespace LinkNest.Application.Abstraction.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
     where TQuery : IQuery<TResponse>
    {

    }
}
