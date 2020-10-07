using Api.Graphql;
using Api.Graphql.Mutations;
using Api.Infrastructure.NHibernate;
using Autofac;
using Commands;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queries;

namespace Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateTodoCommand).Assembly, typeof(GetTodosQuery).Assembly);
            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Graphql.Queries>()
                    .AddMutationType<RootMutation>()
            );
            services.AddSingleton<TodosMutation>();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterAssemblyModules(GetType().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment())
            {
                NHibernateProfiler.Initialize();
                hostApplicationLifetime.ApplicationStopped.Register(NHibernateProfiler.Shutdown);
            }

            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseMiddleware<NhibernateMiddleware>();
            app.UseGraphQL("/graphql");

            if (env.IsDevelopment())
            {
                app
                .UsePlayground("/graphql")
                .UseVoyager("/graphql");
            }
        }
    }
}
