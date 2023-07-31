using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TodoApplication.Domain.Entities;
using TodoApplication.Web.Controllers;
using TodoApplication.Web.Models.Repositories;
using System.Linq;

namespace TodoApplication.Tests
{
    [TestFixture]
    public class TodoTasksControllerTests
    {
        private Mock<ITodoTaskRepository> _mockTaskRepository;
        private Mock<ITodoListRepository> _mockListRepository;
        private TodoTasksController _controller;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITodoTaskRepository>();
            _mockListRepository = new Mock<ITodoListRepository>();
            _controller = new TodoTasksController(_mockTaskRepository.Object, _mockListRepository.Object);
        }

        [Test]
        public async Task Create_WithValidTodoTask_RedirectsToIndex()
        {
            // Arrange
            var todoTask = new TodoTask { Id = 1, Title = "Task 1", ListId = 1 };
            var todoList = new TodoList { Id = 1, Title = "List 1" };
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(new List<TodoList> { todoList }.AsQueryable());
            _mockListRepository.Setup(repo => repo.UpdateAsync(todoList)).Returns(Task.FromResult(todoList));
            _mockTaskRepository.Setup(repo => repo.CreateAsync(todoTask)).Returns(Task.FromResult(todoTask));

            // Act
            var result = await _controller.Create(todoTask) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [Test]
        public async Task Create_WithInvalidTodoTask_ThrowsException()
        {
            // Arrange
            var todoTask = new TodoTask { Id = 1, Title = "Task 1", ListId = 1 };
            _controller.ModelState.AddModelError("Title", "Title is required.");

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _controller.Create(todoTask));
        }

        [Test]
        public async Task Edit_WithValidTodoTask_RedirectsToIndex()
        {
            // Arrange
            var id = 1;
            var todoTask = new TodoTask { Id = id, Title = "Task 1", ListId = 1 };
            var existingTask = new TodoTask { Id = id, Title = "Task 1", ListId = 1 };
            var todoList = new TodoList { Id = 1, Title = "List 1" };
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(new List<TodoList> { todoList }.AsQueryable());
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(id)).Returns(Task.FromResult(existingTask));
            _mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TodoTask>())).Returns(Task.FromResult(existingTask));
            _mockListRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TodoList>())).Returns(Task.FromResult(todoList));

            // Act
            var result = await _controller.Edit(id, todoTask) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [Test]
        public async Task Edit_WithInvalidTodoTask_ReturnsViewResult()
        {
            // Arrange
            var id = 1;
            var todoTask = new TodoTask { Id = id, Title = "Task 1", ListId = 1 };
            _controller.ModelState.AddModelError("Title", "Title is required.");

            // Act
            var result = await _controller.Edit(id, todoTask);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.AreEqual(todoTask, viewResult.Model);

            var expectedViewName = string.IsNullOrWhiteSpace(todoTask.Title) ? "Edit" : null;
            Assert.AreEqual(expectedViewName, viewResult.ViewName);
        }
    }
}
