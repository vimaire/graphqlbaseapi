using System;
using FluentMigrator;

namespace Migrations
{
    [Migration(202010072127)]
    public class CreateTodoTable: AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Todos")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Task").AsAnsiString(255).NotNullable()
                .WithColumn("IsCompleted").AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}
