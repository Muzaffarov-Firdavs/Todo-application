using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApplication.Domain.Entities;
using TodoApplication.Domain.Enums;

namespace TodoApplication.Web.Models.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            TodoApplicationDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<TodoApplicationDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.EnsureCreated();
            }

            if (!context.TodoLists.Any())
            {
                var todoLists = new List<TodoList>
                {
                    new TodoList
                    {
                        Title = "Work",
                        CountOfTasks = 4,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoList
                    {
                        Title = "Education",
                        CountOfTasks = 4,
                        CreatedAt = DateTime.UtcNow
                    }, new TodoList
                    {
                        Title = "Personal",
                        CountOfTasks = 4,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.TodoLists.AddRange(todoLists);
                context.SaveChanges();

                var todoTasks = new List<TodoTask>
                {
                    new TodoTask
                    {
                        Title = "Complete Epam 1st stage",
                        Description = "Finish 1st stage and submit it by the deadline",
                        Status = Status.Completed,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.Today,
                        ListId = todoLists.First(t => t.Title == "Work").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Complete Epam 2nd stage",
                        Description = "Finish 2nd stage and submit it by the deadline",
                        Status = Status.InProgress,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.Today,
                        ListId = todoLists.First(t => t.Title == "Work").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Complete Epam 3nd stage",
                        Description = "Finish 2nd stage and submit it by the deadline",
                        Status = Status.NotStarted,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.UtcNow.AddDays(1),
                        ListId = todoLists.First(t => t.Title == "Work").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Prepare for technical interview",
                        Description = "Review and refresh your knewledge",
                        Status = Status.Completed,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.Today,
                        ListId = todoLists.First(t => t.Title == "Work").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Read a book",
                        Description = "read 50 page book",
                        Status = Status.InProgress,
                        IsImportant = false,
                        IsPlanned = true,
                        DueTime = DateTime.UtcNow.AddDays(1),
                        ListId = todoLists.First(t => t.Title == "Education").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Do shadowing",
                        Description = "do shadowing with Simon Sinek video",
                        Status = Status.Completed,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.Today,
                        ListId = todoLists.First(t => t.Title == "Education").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Learn publish app to Azure",
                        Description = "learn with Microsoft documentation",
                        Status = Status.NotStarted,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.Today,
                        ListId = todoLists.First(t => t.Title == "Education").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Learn CI/CD proccess",
                        Description = "Learn by Microsoft documentation",
                        Status = Status.NotStarted,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.UtcNow.AddDays(5),
                        ListId = todoLists.First(t => t.Title == "Education").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Do meditation",
                        Description = "do meditaion 30 minutes",
                        Status = Status.Completed,
                        IsImportant = false,
                        IsPlanned = true,
                        DueTime = DateTime.UtcNow.AddDays(1),
                        ListId = todoLists.First(t => t.Title == "Personal").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Go familiy gathering",
                        Description = "go family gathering in once a week",
                        Status = Status.NotStarted,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.UtcNow.AddDays(6),
                        ListId = todoLists.First(t => t.Title == "Personal").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Go shopping",
                        Description = "buy bread, sugar and carrots",
                        Status = Status.Completed,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.Today,
                        ListId = todoLists.First(t => t.Title == "Personal").Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new TodoTask
                    {
                        Title = "Learn public speaking",
                        Description = "learn public speaking course at public places",
                        Status = Status.Completed,
                        IsImportant = true,
                        IsPlanned = true,
                        DueTime = DateTime.Today,
                        ListId = todoLists.First(t => t.Title == "Personal").Id,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.TodoTasks.AddRange(todoTasks);
                context.SaveChanges();
            }
        }
    }
}
