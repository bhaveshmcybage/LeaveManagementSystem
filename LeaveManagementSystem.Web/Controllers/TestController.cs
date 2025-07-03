using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
	public class TestController : Controller
	{
		public IActionResult Index()
		{
			var data = new TestViewModel()
            {
				Name = "Student of MVC Master",
				DateOfBirth = new DateTime(1955,08,11)
            };

			return View(data);
		}
	}
}
