using System.Threading;
using System.Threading.Tasks;
using Commands;
using MediatR;

namespace Api.Graphql
{
    public class TodosMutation
    {
        private readonly IMediator _mediator;

        public TodosMutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> CreateTodo(CreateTodoCommand.Todo todo, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreateTodoCommand(todo), cancellationToken);
            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteTodoCommand(id), cancellationToken);
            return true;
        }
    }
}
