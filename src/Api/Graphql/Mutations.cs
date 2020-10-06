using System.Threading;
using System.Threading.Tasks;
using Commands;
using MediatR;

namespace Api.Graphql
{
    public class Mutations
    {
        private readonly IMediator _mediator;

        public Mutations(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> CreateTodo(CreateTodoCommand.Todo todo, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreateTodoCommand(todo), cancellationToken);
            return true;
        }
    }
}
