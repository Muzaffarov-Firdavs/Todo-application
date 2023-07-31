using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApplication.Domain.Entities;

namespace TodoApplication.Web.Models.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly TodoApplicationDbContext _context;

        public TodoTaskRepository(TodoApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<TodoTask> GetAllTasks() => _context.TodoTasks;

        public async Task<TodoTask> GetByIdAsync(long id) => await _context.TodoTasks.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<TodoTask> UpdateAsync(TodoTask todoTask)
        {
            var entry = _context.TodoTasks.Update(todoTask);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<TodoTask> CreateAsync(TodoTask todoTask)
        {
            var enteredEntiy = await _context.TodoTasks.AddAsync(todoTask);
            await _context.SaveChangesAsync();
            return enteredEntiy.Entity;
        }
    }
}
