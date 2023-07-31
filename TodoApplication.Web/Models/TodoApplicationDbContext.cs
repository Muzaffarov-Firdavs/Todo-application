using Microsoft.EntityFrameworkCore;
using TodoApplication.Domain.Entities;

namespace TodoApplication.Web.Models
{
    public class TodoApplicationDbContext : DbContext
    {
        public TodoApplicationDbContext(DbContextOptions<TodoApplicationDbContext> options)
            : base(options) { }

        public DbSet<TodoTask> TodoTasks { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoTask>()
                .HasOne(todoTask => todoTask.List)
                .WithMany(todoList => todoList.Tasks)
                .HasForeignKey(discount => discount.ListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
