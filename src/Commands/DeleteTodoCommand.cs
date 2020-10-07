using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using NHibernate;

namespace Commands
{
    public class DeleteTodoCommand : IRequest
    {
        public DeleteTodoCommand(long id)
        {
            Id = id;
        }

        public long Id { get;  }
    }

    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly ISessionFactory _sessionFactory;

        public DeleteTodoCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var session = _sessionFactory.GetCurrentSession();
            var item = await session.GetAsync<Todo>(request.Id, cancellationToken);
            await session.DeleteAsync(item, cancellationToken);
            return Unit.Value;
        }
    }
}