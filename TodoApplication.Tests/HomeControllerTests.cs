using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TodoApplication.Domain.Entities;
using TodoApplication.Domain.Enums;
using TodoApplication.Web.Controllers;
using TodoApplication.Web.Models.Repositories;
using TodoApplication.Web.Models.ViewModels;

namespace TodoApplication.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private Mock<ITodoTaskRepository> _mockTaskRepository;
        private Mock<ITodoListRepository> _mockListRepository;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITodoTaskRepository>();
            _mockListRepository = new Mock<ITodoListRepository>();

            _controller = new HomeController(_mockTaskRepository.Object, _mockListRepository.Object);
        }

        [Test]
        public void Index_WithNullTitle_ReturnsAllTasks()
        {
            // Arrange
            var tasks = new List<TodoTask>
            {
                new TodoTask { Id = 1, List = new TodoList { Title = "List 1" } },
                new TodoTask { Id = 2, List = new TodoList { Title = "List 2" } },
                new TodoTask { Id = 3, List = new TodoList { Title = "List 1" } },
            };
            var querableTasks = tasks.AsQueryable();
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Returns(querableTasks);

            // Act
            var result = _controller.Index(null, 1) as ViewResult;
            var model = result.Model as TodoTasksViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(3, model.TodoTasks.Count());
            Assert.AreEqual(null, model.CurrentTodoList);
            Assert.AreEqual(1, model.PagingInfo.CurrentPage);
            Assert.AreEqual(HomeController.PageSize, model.PagingInfo.ItemsPerPage);
            Assert.AreEqual(3, model.PagingInfo.TotalItems);
        }

        [Test]
        public void Index_WithTitle_ReturnsFilteredTasks()
        {
            // Arrange
            var tasks = new List<TodoTask>
            {
                new TodoTask { Id = 1, List = new TodoList { Title = "List 1" } },
                new TodoTask { Id = 2, List = new TodoList { Title = "List 2" } },
                new TodoTask { Id = 3, List = new TodoList { Title = "List 1" } },
            };
            var querableTasks = tasks.AsQueryable();
            _mockTaskRepository.Setup(repo => repo.GetAllTasks()).Returns(querableTasks);

            var lists = new List<TodoList>
            {
                new TodoList { Title = "List 1" },
                new TodoList { Title = "List 2" },
            };
            var querableLists = lists.AsQueryable();
            _mockListRepository.Setup(repo => repo.GetAllLists()).Returns(querableLists);

            // Act
            var result = _controller.Index("List 1", 1) as ViewResult;
            var model = result.Model as TodoTasksViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(2, model.TodoTasks.Count());
            Assert.AreEqual("List 1", model.CurrentTodoList.Title);
            Assert.AreEqual(1, model.PagingInfo.CurrentPage);
            Assert.AreEqual(HomeController.PageSize, model.PagingInfo.ItemsPerPage);
            Assert.AreEqual(2, model.PagingInfo.TotalItems);
        }

        [Test]
        public void GetCompletedTasks_ReturnsViewResult()
        {
            // Arrange
            var completedTasks = new List<TodoTask>
            {
                new TodoTask { Id = 1, Title = "Task 1", Status = Status.Completed },
                new TodoTask { Id = 2, Title = "Task 2", Status = Status.Completed }
            };

            _mockTaskRepository.Setup(repo => repo.GetAllTasks())
                              .Returns(completedTasks.AsQueryable());

            // Act
            var result = _controller.GetCompletedTasks() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewModel = result.Model as TodoTasksViewModel;
            Assert.NotNull(viewModel);

            var tasks = viewModel.TodoTasks.ToList();
            Assert.AreEqual(completedTasks.Count, tasks.Count);
            Assert.IsTrue(tasks.All(t => t.Status == Status.Completed));
            Assert.AreEqual(completedTasks.Count, viewModel.PagingInfo.TotalItems);
        }

        [Test]
        public void GetTodayTasks_ReturnsViewResult()
        {
            // Arrange
            var todayTasks = new List<TodoTask>
            {
                new TodoTask { Id = 1, Title = "Task 1", DueTime = DateTime.Today },
                new TodoTask { Id = 2, Title = "Task 2", DueTime = DateTime.Today }
            };

            _mockTaskRepository.Setup(repo => repo.GetAllTasks())
                              .Returns(todayTasks.AsQueryable());

            // Act
            var result = _controller.GetTodayTasks() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewModel = result.Model as TodoTasksViewModel;
            Assert.NotNull(viewModel);

            var tasks = viewModel.TodoTasks.ToList();
            Assert.AreEqual(todayTasks.Count, tasks.Count);
            Assert.IsTrue(tasks.All(t => t.DueTime != null && t.DueTime.Value.Date == DateTime.Today));

            Assert.AreEqual(todayTasks.Count, viewModel.PagingInfo.TotalItems);
        }

    }
}
