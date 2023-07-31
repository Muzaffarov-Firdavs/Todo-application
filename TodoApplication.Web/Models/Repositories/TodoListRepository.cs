using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Domain.Entities;

namespace TodoApplication.Web.Models.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly TodoApplicationDbContext _context;

        public TodoListRepository(TodoApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<TodoList> GetAllLists() => _context.TodoLists;

        public async Task<TodoList> GetByIdAsync(long id) => await _context.TodoLists.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<TodoList> UpdateAsync(TodoList todoList)
        {
            var entry = _context.TodoLists.Update(todoList);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<TodoList> CreateAsync(TodoList todoList)
        {
            var enteredEntiy = await _context.TodoLists.AddAsync(todoList);
            await _context.SaveChangesAsync();
            return enteredEntiy.Entity;
        }

        public async Task DeleteAsync(long id)
        {
            var existEntity = await _context.TodoLists.FirstOrDefaultAsync(p =>p.Id == id);
            _context.TodoLists.Remove(existEntity);
            await _context.SaveChangesAsync();
        }
    }
}
