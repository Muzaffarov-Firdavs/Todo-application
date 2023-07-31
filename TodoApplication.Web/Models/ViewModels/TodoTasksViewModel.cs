using System.Collections.Generic;
using TodoApplication.Domain.Entities;

namespace TodoApplication.Web.Models.ViewModels
{
    public class TodoTasksViewModel
    {
        public IEnumerable<TodoTask> TodoTasks { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public TodoList CurrentTodoList { get; set; }
    }
}
