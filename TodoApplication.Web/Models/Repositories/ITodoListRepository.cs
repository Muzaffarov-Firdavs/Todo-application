using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Domain.Entities;

namespace TodoApplication.Web.Models.Repositories
{
    public interface ITodoListRepository
    {
        IQueryable<TodoList> GetAllLists();
        Task<TodoList> GetByIdAsync(long id);
        Task<TodoList> UpdateAsync(TodoList todoList);
        Task<TodoList> CreateAsync(TodoList todoList);
        Task DeleteAsync(long id);
    }
}
