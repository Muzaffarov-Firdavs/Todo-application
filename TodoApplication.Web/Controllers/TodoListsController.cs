using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Domain.Entities;
using TodoApplication.Web.Models.Repositories;
using TodoApplication.Web.Models.ViewModels;

namespace TodoApplication.Web.Controllers
{
    public class TodoListsController : Controller
    {
        public const int PageSize = 10;
        private readonly ITodoTaskRepository _todoTaskRepository;
        private readonly ITodoListRepository _todoListRepository;

        public TodoListsController(ITodoTaskRepository todoTaskRepository, ITodoListRepository todoListRepository)
        {
            _todoTaskRepository = todoTaskRepository;
            _todoListRepository = todoListRepository;
        }

        public IActionResult Index(int taskPage = 1)
        {
            return View(new TodoListsViewModel
            {
                TodoLists = _todoListRepository.GetAllLists()
                                    .OrderBy(p => p.Id)
                                    .Skip((taskPage - 1) * PageSize)
                                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = taskPage,
                    ItemsPerPage = PageSize,
                    TotalItems = _todoListRepository.GetAllLists().Count()
                }
            });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] TodoList todoList)
        {
            if (!ModelState.IsValid)
                return View(todoList);

            if (string.IsNullOrWhiteSpace(todoList.Title))
                return View(todoList);

            var existList = _todoListRepository.GetAllLists()
                .FirstOrDefault(p => p.Title.ToLower() == todoList.Title.ToLower());
            if (existList is null)
            {
                todoList.CreatedAt = DateTime.UtcNow;
                await _todoListRepository.CreateAsync(todoList);
                return RedirectToAction("Index", "Home");
            }

            throw new Exception("TodoList already exist.");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _todoListRepository.GetByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoList todoList)
        {
            if (!ModelState.IsValid)
                return View(todoList);

            if (string.IsNullOrWhiteSpace(todoList.Title))
                return View(todoList);

            var ckeckList = _todoListRepository.GetAllLists()
                .FirstOrDefault(p => p.Title.ToLower() == todoList.Title.ToLower());
            if (ckeckList is null)
            {
                var existList = await _todoListRepository.GetByIdAsync(id);

                existList.Title = todoList.Title;
                existList.UpdatedAt = DateTime.UtcNow;
                await _todoListRepository.UpdateAsync(existList);
                return RedirectToAction("Index", "Home");
            }

            throw new Exception("TodoList already exist with this title.");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _todoListRepository.GetByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, TodoList todoList)
        {
            var existList = await _todoListRepository.GetByIdAsync(id);
            if (existList is null)
                return NotFound("Todo list not found.");

            await _todoListRepository.DeleteAsync(id);
            return RedirectToAction("Index", "TodoLists");
        }
    }
}
