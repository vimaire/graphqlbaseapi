using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Api.Infrastructure.NHibernate;
using Autofac;
using Domain;
using Microsoft.Extensions.Configuration;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Api.Infrastructure.Ioc
{
    public class NhibernateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var configuration = context.Resolve<IConfiguration>();
                    var connectionString = configuration.GetConnectionString("Default");

                    var cfg = new Configuration().DataBaseIntegration(db =>
                    {
                        db.ConnectionString = connectionString;
                        db.Driver<NpgsqlDriver>();
                        db.Dialect<PostgreSQL83Dialect>();
                        db.BatchSize = 100;
                        db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                        db.IsolationLevel = IsolationLevel.ReadCommitted;
                        db.AutoCommentSql = true;
                        db.KeywordsAutoImport = Hbm2DDLKeyWords.None;
                    });
                    cfg.SetNamingStrategy(new PostgresNamingStrategy());
                    cfg.CurrentSessionContext<LazySessionContext>();
                    var mapper = new ModelMapper();
                    mapper.AddMappings(typeof(Todo).Assembly.GetTypes());
                    cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

                    var sessionFactory = cfg.BuildSessionFactory();
                    return sessionFactory;
                })
                .SingleInstance()
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}
