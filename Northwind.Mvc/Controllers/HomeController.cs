using Microsoft.AspNetCore.Mvc; //Controller, IActionResult
using Northwind.Mvc.Models; //ErrorViewModel
using System.Diagnostics; //Activity
using Packt.Shared;
using Microsoft.AspNetCore.Authorization;

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

		public IActionResult ProductDetail(int? id) //model binding binds the id from route to parameter passed here
		{
			//here he doesnt use a viewmodel, aparently it is not needed, since we only need a product
			if (!id.HasValue)
			{
				return BadRequest("Product Id missing");
			}

			Product? model = db.Products.SingleOrDefault(p => p.ProductId == id);

			if (model is null)
			{
				return NotFound($"Product {id} not found.");
			}

			return View(model);
		}

		[Authorize(Roles = "Administrators")]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult ModelBinding()
		{
			return View(); //page with a form to submit
        }

		[HttpPost] // use this action method to process POSTs (since the methods are called the same)
        public IActionResult ModelBinding(Thing thing)
        {
			HomeModelBindingViewModel model = new(
			Thing: thing, 
			HasErrors: !ModelState.IsValid,
			ValidationErrors: ModelState.Values
				.SelectMany(state => state.Errors)
				.Select(error => error.ErrorMessage)
			);

			return View(model);
		}
    }
}
