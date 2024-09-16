using Microsoft.AspNetCore.Mvc; //Controller, IActionResult
using Northwind.Mvc.Models; //ErrorViewModel
using System.Diagnostics; //Activity
using Packt.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Mvc.Controllers;

public class HomeController(ILogger<HomeController> logger, NorthwindContext injectedContext) : Controller
{
	private readonly ILogger<HomeController> _logger = logger;
	private readonly NorthwindContext db = injectedContext;

	//this will store the HTTP Response in cache for 10 seconds
	//this dont work with antiforgery tokens, so it wont work if logged in
	[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
	public async Task<IActionResult> Index()
	{
		//writing to the console:
		//_logger.LogError("terrible error over here!");
		//_logger.LogWarning("i warn you,this is very serious");
		//_logger.LogInformation("writing from the Index method inside the Home controller");

		HomeIndexViewModel model = new
		(
			VisitorCount: Random.Shared.Next(1, 1001), //Random.Shared requires no instanciation!
			Categories: await db.Categories.ToListAsync(),
			Products: await db.Products.ToListAsync()
		);
		//when View is called, it looks in the Views folder for the view file, and also in the shared folder and razor pages shared folder.
		return View(model);
	}

	public async Task<IActionResult> ProductDetail(int? id, string alertstyle = "success") //model binding binds the id from route to parameter passed here, query string value = alertstyle
	{
		//adding the query string value to ViewData... 
		ViewData["alertstyle"] = alertstyle;
		//here he doesnt use a viewmodel, aparently it is not needed, since we only need a product
		if (!id.HasValue)
		{
			return BadRequest("Product Id missing");
		}

		Product? model = await db.Products.SingleOrDefaultAsync(p => p.ProductId == id); 

		if (model is null)
		{
			return NotFound($"Product {id} not found.");
		}

		return View(model);
	}

	[Route("private")]
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

    public IActionResult ProductsThatCostMoreThan(decimal? price)
    {
            if (!price.HasValue)
            {
                return BadRequest("You must pass a product price in the query string,for example, / Home / ProductsThatCostMoreThan ? price = 50");
            }
            IEnumerable<Product> model = db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => p.UnitPrice > price);
            if (!model.Any())
            {
                return NotFound($"No products cost more than {price:C}.");
            }
            ViewData["MaxPrice"] = price.Value.ToString("C");
            return View(model); // pass model to view
    }
}
