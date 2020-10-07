using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using NHibernate;

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
        private readonly ISessionFactory _sessionFactory;

        public GetTodosQueryHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Task<IQueryable<GetTodosQuery.Todo>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var session = _sessionFactory.GetCurrentSession();
            var queryable = session.Query<Todo>().Select(x => new GetTodosQuery.Todo
            {
                Id = x.Id,
                Task = x.Task,
                IsCompleted = x.IsCompleted
            });

            return Task.FromResult(queryable);
        }
    }
}
