using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using TodoApplication.Domain.Entities;
using TodoApplication.Web.Models.Repositories;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TodoApplication.Web.Controllers
{
    public class TodoTasksController : Controller
    {
        private readonly ITodoTaskRepository _todoTaskRepository;
        private readonly ITodoListRepository _todoListRepository;

        public TodoTasksController(ITodoTaskRepository todoTaskRepository, ITodoListRepository todoListRepository)
        {
            _todoTaskRepository = todoTaskRepository;
            _todoListRepository = todoListRepository;
        }

        public IActionResult Create()
        {
            var todoLists = _todoListRepository.GetAllLists(); 

            ViewBag.TodoLists = todoLists;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,Status,IsImportant,IsPlanned,DueTime,ListId")] TodoTask todoTask)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Task is not valid.");
            }

            var todoList = _todoListRepository.GetAllLists()
                .FirstOrDefault(p => p.Id == todoTask.ListId);
            if (todoList is null)
                throw new Exception("List title is not true.");

            todoList.CountOfTasks += 1;
            todoList.CreatedAt = DateTime.UtcNow;
            await _todoListRepository.UpdateAsync(todoList);

            todoTask.ListId = todoList.Id;
            await _todoTaskRepository.CreateAsync(todoTask);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var todoLists = _todoListRepository.GetAllLists();

            ViewBag.TodoLists = todoLists;

            var product = await _todoTaskRepository.GetByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoTask todoTask)
        {
            if (!ModelState.IsValid)
                return View(todoTask);

            if (string.IsNullOrWhiteSpace(todoTask.Title))
                return View(todoTask);

            var existTask = await _todoTaskRepository.GetByIdAsync(id);
            if (existTask is null)
                throw new Exception("TodoList not found.");
            
            if  (existTask.ListId != todoTask.ListId)
            {
                var lastList = await _todoListRepository.GetAllLists()
                    .FirstOrDefaultAsync(p => p.Id == existTask.ListId);
                lastList.CountOfTasks -= 1;
                await _todoListRepository.UpdateAsync(lastList);

                var newList = await _todoListRepository.GetAllLists()
                    .FirstOrDefaultAsync(p => p.Id == todoTask.ListId);
                newList.CountOfTasks += 1;
                await _todoListRepository.UpdateAsync(newList);
            }

            existTask.Title = todoTask.Title;
            existTask.Description = todoTask.Description;
            existTask.Status = todoTask.Status;
            existTask.IsImportant = todoTask.IsImportant;
            existTask.IsPlanned = todoTask.IsPlanned;
            existTask.DueTime = todoTask.DueTime;
            existTask.ListId = todoTask.ListId;
            existTask.CreatedAt = existTask.CreatedAt;
            existTask.UpdatedAt = DateTime.UtcNow;
            await _todoTaskRepository.UpdateAsync(existTask);
            return RedirectToAction("Index", "Home");
        }
    }
}
