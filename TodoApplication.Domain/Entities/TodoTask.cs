using System;
using TodoApplication.Domain.Commons;
using TodoApplication.Domain.Enums;

namespace TodoApplication.Domain.Entities
{
    public class TodoTask : Auditable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public bool IsImportant { get; set; }
        public bool IsPlanned { get; set; }
        public DateTime? DueTime { get; set; }

        public long ListId { get; set; }
        public TodoList List { get; set; }
    }
}
