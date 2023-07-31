using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApplication.Domain.Enums;
using TodoApplication.Web.Models.Repositories;

namespace TodoApplication.Web.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ITodoTaskRepository _todoTaskRepository;

        public NavigationMenuViewComponent(ITodoTaskRepository todoTaskRepository)
        {
            _todoTaskRepository = todoTaskRepository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.CompletedCategory = _todoTaskRepository.GetAllTasks()
                .Where(p => p.Status == Status.Completed);

            ViewBag.CountOfCompletedTaks = _todoTaskRepository.GetAllTasks()
                .Where(p => p.Status == Status.Completed)
                .Count();

            ViewBag.SelectedCategory = RouteData?.Values["list"];
            return View(_todoTaskRepository.GetAllTasks()
               .Select(x => x.List)
               .Distinct()
               .OrderBy(x => x));
        }
    }
}
