using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Commands
{
    public class CreateTodoCommand : IRequest
    {
        public Todo Data { get; }
        public CreateTodoCommand(Todo data)
        {
            Data = data;
        }

        public class Todo
        {
            public string Task { get; set; }
        }
    }

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand>
    {
        public Task<Unit> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
