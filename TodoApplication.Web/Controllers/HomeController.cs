using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TodoApplication.Domain.Enums;
using TodoApplication.Web.Models.Repositories;
using TodoApplication.Web.Models.ViewModels;

namespace TodoApplication.Web.Controllers
{
    public class HomeController : Controller
    {
        public const int PageSize = 4;
        private readonly ITodoTaskRepository _todoTaskRepository;
        private readonly ITodoListRepository _todoListRepository;

        public HomeController(ITodoTaskRepository todoTaskRepository, ITodoListRepository todoListRepository)
        {
            _todoTaskRepository = todoTaskRepository ?? throw new ArgumentNullException(nameof(todoTaskRepository));
            _todoListRepository = todoListRepository ?? throw new ArgumentNullException(nameof(todoListRepository));
        }

        public ViewResult Index(string title, int taskPage = 1)
        {
            return View(new TodoTasksViewModel
            {
                TodoTasks = _todoTaskRepository.GetAllTasks()
                                    .Where(p => p.Status != Status.Completed && title == null || p.List.Title == title)
                                    .OrderByDescending(p => p.Id)
                                    .Skip((taskPage - 1) * PageSize)
                                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = taskPage,
                    ItemsPerPage = PageSize,
                    TotalItems = title == null ?
                         _todoTaskRepository.GetAllTasks()
                                .Where(e => e.Status != Status.Completed).Count() :
                         _todoTaskRepository.GetAllTasks()
                                .Where(e => e.List.Title == title && e.Status != Status.Completed).Count()
                },
                CurrentTodoList = _todoListRepository.GetAllLists().FirstOrDefault(p => p.Title == title)
            });
        }

        public ViewResult GetCompletedTasks(string title = "CompletedTasks", int taskPage = 1)
        {
            return View(new TodoTasksViewModel
            {
                TodoTasks = _todoTaskRepository.GetAllTasks()
                                    .Where(p => p.Status == Status.Completed)
                                    .OrderByDescending(p => p.Id)
                                    .Skip((taskPage - 1) * PageSize)
                                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = taskPage,
                    ItemsPerPage = PageSize,
                    TotalItems = _todoTaskRepository.GetAllTasks().Where(e =>
                             e.Status == Status.Completed).Count()
                }
            });
        }

        public ViewResult GetTodayTasks(string title = "TodayTasks", int taskPage = 1)
        {
            return View(new TodoTasksViewModel
            {
                TodoTasks = _todoTaskRepository.GetAllTasks()
                                    .Where(p => p.DueTime != null && p.DueTime.Value.Date == DateTime.Today)
                                    .OrderByDescending(p => p.Id)
                                    .Skip((taskPage - 1) * PageSize)
                                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = taskPage,
                    ItemsPerPage = PageSize,
                    TotalItems = _todoTaskRepository.GetAllTasks().Where(e =>
                             e.DueTime == DateTime.Today).Count()
                }
            });
        }
    }
}