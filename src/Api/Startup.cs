using Commands;
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
                    .AddMutationType<Graphql.Mutations>()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

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
