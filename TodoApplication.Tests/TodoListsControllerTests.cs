using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApplication.Domain.Entities;
using TodoApplication.Web.Controllers;
using TodoApplication.Web.Models.Repositories;
using TodoApplication.Web.Models.ViewModels;

namespace TodoApplication.Tests
{
    [TestFixture]
    public class TodoListsControllerTests
    {
        private TodoListsController _controller;
        private Mock<ITodoTaskRepository> _mockTaskRepository;
        private Mock<ITodoListRepository> _mockListRepository;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITodoTaskRepository>();
            _mockListRepository = new Mock<ITodoListRepository>();

            _controller = new TodoListsController(_mockTaskRepository.Object, _mockListRepository.Object);
        }

        [Test]
        public void Index_ReturnsViewResultWithAllLists()
        {
            // Arrange
            var lists = new List<TodoList>
            {
                new TodoList { Id = 1, Title = "List 1" },
                new TodoList { Id = 2, Title = "List 2" },
                new TodoList { Id = 3, Title = "List 3" }
            };
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(lists.AsQueryable());

            // Act
            var result = _controller.Index(1) as ViewResult;
            var model = result.Model as TodoListsViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(3, model.TodoLists.Count());
            Assert.AreEqual(1, model.PagingInfo.CurrentPage);
            Assert.AreEqual(TodoListsController.PageSize, model.PagingInfo.ItemsPerPage);
            Assert.AreEqual(3, model.PagingInfo.TotalItems);
        }

        [Test]
        public async Task Create_WithValidTodoList_RedirectsToIndex()
        {
            // Arrange
            var todoList = new TodoList { Id = 1, Title = "List 1" };
            var todoLists = new List<TodoList>();
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(todoLists.AsQueryable());
            _mockListRepository.Setup(repo => repo.CreateAsync(todoList)).Returns(Task.FromResult(todoList));

            // Act
            var result = await _controller.Create(todoList) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }


        [Test]
        public async Task Create_WithExistingTodoList_ThrowErrorMessage()
        {
            // Arrange
            var todoList = new TodoList { Id = 1, Title = "List 1" };
            var existingList = new TodoList { Id = 2, Title = "List 1" };
            var todoLists = new List<TodoList> { existingList };
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(todoLists.AsQueryable());

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _controller.Create(todoList));

            // Assert
            Assert.AreEqual("TodoList already exist.", exception.Message);
        }

        [Test]
        public async Task Edit_WithValidTodoList_RedirectsToIndex()
        {
            // Arrange
            var todoList = new TodoList { Id = 1, Title = "List 1" };
            var existingList = new TodoList { Id = 1, Title = "Existing List" };
            var todoLists = new List<TodoList> { existingList };
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(todoLists.AsQueryable());
            _mockListRepository.Setup(repo => repo.GetByIdAsync(1)).Returns(Task.FromResult(existingList));
            _mockListRepository.Setup(repo => repo.UpdateAsync(existingList)).Returns(Task.FromResult(todoList));

            // Act
            var result = await _controller.Edit(1, todoList) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [Test]
        public async Task Edit_WithExistingTodoList_ThrowErrorMessage()
        {
            // Arrange
            var id = 1;
            var todoList = new TodoList { Id = id, Title = "List 1" };
            var existingList = new TodoList { Id = 2, Title = "List 1" };
            var todoLists = new List<TodoList> { existingList };
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(todoLists.AsQueryable());
            _mockListRepository.Setup(repo => repo.GetByIdAsync(id)).Returns(Task.FromResult(existingList));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _controller.Edit(id, todoList));

            // Assert
            Assert.AreEqual("TodoList already exist with this title.", exception.Message);
        }

        [Test]
        public async Task Delete_ExistingTodoList_RedirectsIndex()
        {
            // Arrange
            var id = 1;
            var todoList = new TodoList { Id = id };

            _mockListRepository.Setup(repo => repo.GetByIdAsync(id)).Returns(Task.FromResult(todoList));
            _mockListRepository.Setup(repo => repo.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id, todoList) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("TodoLists", result.ControllerName);
        }

        [Test]
        public async Task Delete_WithWrongId_ReturnsNotFound()
        {
            // Arrange
            var id = 1;
            TodoList todoList = null;

            _mockListRepository.Setup(repo => repo.GetByIdAsync(id)).Returns(Task.FromResult(todoList));

            // Act
            var result = await _controller.Delete(id, null);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Todo list not found.", notFoundResult.Value);
        }
    }
}
