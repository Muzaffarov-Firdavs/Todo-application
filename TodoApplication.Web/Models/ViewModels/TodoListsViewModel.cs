using System.Collections.Generic;
using TodoApplication.Domain.Entities;

namespace TodoApplication.Web.Models.ViewModels
{
    public class TodoListsViewModel
    {
        public IEnumerable<TodoList> TodoLists { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
