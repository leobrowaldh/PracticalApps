using Microsoft.AspNetCore.Mvc; //Controller, IActionResult
using Northwind.Mvc.Models; //ErrorViewModel
using System.Diagnostics; //Activity
using Packt.Shared;

namespace Northwind.Mvc.Controllers
{
	public class HomeController(ILogger<HomeController> logger, NorthwindContext injectedContext) : Controller
	{
		private readonly ILogger<HomeController> _logger = logger;
		private readonly NorthwindContext db = injectedContext;

		public IActionResult Index()
		{
			//writing to the console:
			//_logger.LogError("terrible error over here!");
			//_logger.LogWarning("i warn you,this is very serious");
			//_logger.LogInformation("writing from the Index method inside the Home controller");

			HomeIndexViewModel model = new
			(
				VisitorCount: Random.Shared.Next(1, 1001), //Random.Shared requires no instanciation!
				Categories: db.Categories.ToList(), 
				Products: [.. db.Products] //this is the new collection expression, its the same as above, and it infers the type (list)
			);
			//when View is called, it looks in the Views folder for the view file, and also in the shared folder and razor pages shared folder.
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
