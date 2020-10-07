using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NHibernate;
using ISession = NHibernate.ISession;

namespace Api.Infrastructure.NHibernate
{
    public class NhibernateMiddleware
    {
        private readonly RequestDelegate _next;

        public NhibernateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ISessionFactory sessionFactory)
        {
            var initializer = new Lazy<ISession>(() => BeginSession(sessionFactory));

            LazySessionContext.Bind(initializer, sessionFactory);
            try
            {
                await _next.Invoke(context);
                var session = LazySessionContext.UnBind(sessionFactory);
                await CommitTransactionAsync(session, context.RequestAborted);
            }
            catch
            {
                // Make sure the session is unbounded
                LazySessionContext.UnBind(sessionFactory);

                // Get session from initializer because it can already be unbounded
                await RollbackTransactionAsync(initializer, context.RequestAborted);
                throw;
            }
        }

        private ISession BeginSession(ISessionFactory sessionFactory)
        {
            var session = sessionFactory.OpenSession();
            session.BeginTransaction();
            return session;
        }
        private async Task CommitTransactionAsync(ISession session, CancellationToken cancellationToken)
        {
            if (session != null)
            {
                var tx = session.GetCurrentTransaction();
                if (tx != null && tx.IsActive)
                {
                    await tx.CommitAsync(cancellationToken);
                }
                session.Dispose();
            }
        }
        private static async Task RollbackTransactionAsync(Lazy<ISession> initializer, CancellationToken cancellationToken)
        {
            if (initializer.IsValueCreated)
            {
                var session = initializer.Value;
                var tx = session.GetCurrentTransaction();
                if (tx != null)
                {
                    await tx.RollbackAsync(cancellationToken);
                }

                session.Dispose();
            }
        }
    }
}
