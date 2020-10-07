using Domain;

namespace Api.Graphql.Mutations
{
    public class RootMutation
    {
        private readonly TodosMutation _todos;

        public RootMutation(TodosMutation todos)
        {
            _todos = todos;
        }

        public TodosMutation Todos => _todos;
    }
}