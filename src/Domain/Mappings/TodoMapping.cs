using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class TodoMapping : ClassMapping<Todo>
    {
        public TodoMapping()
        {
            Table("Todos");
            DynamicInsert(true);
            DynamicUpdate(true);
            Id(x => x.Id, mapper => mapper.Generator(Generators.Guid));
            Property(x => x.Task);
            Property(x => x.IsCompleted);
        }
    }
}