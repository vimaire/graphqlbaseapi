using System;

namespace Domain
{
    public class Todo
    {
        protected Todo()
        {
            // Required by Nhibernate
        }

        public Todo(string task)
        {
            Task = task;
            IsCompleted = false;
        }

        public virtual Guid Id { get; protected set; }
        public virtual string Task { get; protected set; }
        public virtual bool IsCompleted { get; protected set; }
    }
}
