using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;

namespace Application.Abstractions.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    {
        Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
    }

}