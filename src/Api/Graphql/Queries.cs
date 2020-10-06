using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Queries;

namespace Api.Graphql
{
    public class Queries
    {
        private readonly IMediator _mediator;

        public Queries(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IQueryable<GetTodosQuery.Todo>> Todos(CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetTodosQuery(), cancellationToken);
        }
    }
}