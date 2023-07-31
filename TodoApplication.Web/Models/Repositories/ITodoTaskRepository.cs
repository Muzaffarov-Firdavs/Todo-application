using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Domain.Commons;
using TodoApplication.Domain.Entities;

namespace TodoApplication.Web.Models.Repositories
{
    public interface ITodoTaskRepository
    {
        IQueryable<TodoTask> GetAllTasks();
        Task<TodoTask> GetByIdAsync(long id);
        Task<TodoTask> UpdateAsync(TodoTask todoTask);
        Task<TodoTask> CreateAsync(TodoTask todoTask);
    }
}
