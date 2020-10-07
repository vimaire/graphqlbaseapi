using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrations;
using Oakton;
using Oakton.AspNetCore;

namespace Api.Infrastructure.Ops
{
    [Description("Run database migrations")]
    public class MigrateCommand : OaktonCommand<MigrateCommand.MigrateInput>
    {
        public class MigrateInput : NetCoreInput
        {
            [Description("Migration direction")]
            public MigrationDirection Direction { get; set; } = MigrationDirection.Up;

            [Description("Target version")]
            [FlagAlias("version", 'v')]
            public long? VersionFlag { get; set; }

            public enum MigrationDirection
            {
                Up,
                Down
            }
        }


        public MigrateCommand()
        {
            Usage("Default migration (up)").ValidFlags();
            Usage("Migrate all forward or one step backward").Arguments(x => x.Direction).ValidFlags();
            Usage("Migrate to a specified direction until the given version").Arguments(x => x.Direction).ValidFlags(x => x.VersionFlag);
        }
        public override bool Execute(MigrateInput input)
        {
            string connectionString;
            using (var host = input.BuildHost())
            {
                var configuration = host.Services.GetRequiredService<IConfiguration>();
                connectionString = configuration.GetConnectionString("Default");
            }

            var serviceProvider = CreateServices(connectionString);

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(input, scope.ServiceProvider);
            }

            return true;
        }

        private void UpdateDatabase(MigrateInput input, IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            switch (input.Direction)
            {
                case MigrateInput.MigrationDirection.Down:
                    if (input.VersionFlag.HasValue)
                    {
                        runner.MigrateDown(input.VersionFlag.Value);
                    }
                    else
                    {
                        runner.Rollback(1);
                    }
                    break;
                case MigrateInput.MigrationDirection.Up:
                    if (input.VersionFlag.HasValue)
                    {
                        runner.MigrateUp(input.VersionFlag.Value);
                    }
                    else
                    {
                        runner.MigrateUp();
                    }
                    break;
            }
        }

        public IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(CreateTodoTable).Assembly).For.Migrations()
                )
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider();
        }
    }
}