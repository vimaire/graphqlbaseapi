using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace Api.Infrastructure.NHibernate
{
    // Source: https://nhibernate.info/blog/2011/03/02/effective-nhibernate-session-management-for-web-apps.html
    public class LazySessionContext : ICurrentSessionContext
    {
        private static readonly AsyncLocal<IDictionary<ISessionFactory, Lazy<ISession>>> CurrentFactoryMap = new AsyncLocal<IDictionary<ISessionFactory, Lazy<ISession>>>();
        private readonly ISessionFactoryImplementor _factory;

        public LazySessionContext(ISessionFactoryImplementor factory)
        {
            _factory = factory;
        }
        public ISession CurrentSession()
        {
            var currentSessionFactoryMap = GetCurrentFactoryMap();
            if (currentSessionFactoryMap == null ||
                !currentSessionFactoryMap.TryGetValue(_factory, out var initializer))
            {
                return null;
            }

            return initializer.Value;
        }

        /// <summary>
        /// Provides the CurrentMap of SessionFactories.
        /// If there is no map create/store and return a new one.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<ISessionFactory, Lazy<ISession>> GetCurrentFactoryMap()
        {
            var currentFactoryMap = CurrentFactoryMap.Value;
            if (currentFactoryMap == null)
            {
                currentFactoryMap = new Dictionary<ISessionFactory, Lazy<ISession>>();
                CurrentFactoryMap.Value = currentFactoryMap;
            }
            return currentFactoryMap;
        }

        /// <summary>
        /// Bind a new sessionInitializer to the context of the sessionFactory.
        /// </summary>
        /// <param name="sessionInitializer"></param>
        /// <param name="sessionFactory"></param>
        public static void Bind(Lazy<ISession> sessionInitializer, ISessionFactory sessionFactory)
        {
            var map = GetCurrentFactoryMap();
            map[sessionFactory] = sessionInitializer;
        }

        /// <summary>
        /// Unbind the current session of the session factory.
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <returns></returns>
        public static ISession UnBind(ISessionFactory sessionFactory)
        {
            var map = GetCurrentFactoryMap();
            var sessionInitializer = map[sessionFactory];
            map[sessionFactory] = null;
            if (sessionInitializer == null || !sessionInitializer.IsValueCreated)
            {
                return null;
            }

            return sessionInitializer.Value;
        }
    }
}
