using System;
using System.Collections.Generic;
using System.Text;
using TodoApplication.Domain.Commons;

namespace TodoApplication.Domain.Entities
{
    public class TodoList : Auditable
    {
        public string Title { get; set; }
        public int CountOfTasks { get; set; }
        public ICollection<TodoTask> Tasks { get; set; }
    }
}
