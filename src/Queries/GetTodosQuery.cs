using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Queries
{
    public class GetTodosQuery : IRequest<IQueryable<GetTodosQuery.Todo>>
    {
        public class Todo
        {
            public Guid Id { get; set; }
            public string Task { get; set; }
            public bool IsCompleted { get; set; }
        }
    }

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IQueryable<GetTodosQuery.Todo>>
    {
        public Task<IQueryable<GetTodosQuery.Todo>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
