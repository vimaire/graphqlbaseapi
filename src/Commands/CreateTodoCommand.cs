using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using NHibernate;

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
        private readonly ISessionFactory _sessionFactory;

        public CreateTodoCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task<Unit> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var session = _sessionFactory.GetCurrentSession();
            var todo = new Todo(request.Data.Task);
            await session.SaveAsync(todo, cancellationToken);
            return Unit.Value;
        }
    }
}
