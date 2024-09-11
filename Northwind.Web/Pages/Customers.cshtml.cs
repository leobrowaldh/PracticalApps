using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Packt.Shared;

namespace Northwind.Web.Pages
{
    public class CustomersModel(NorthwindContext injectedContext) : PageModel
    {
		public IEnumerable<Customer>? Customers { get; set; }
		private readonly NorthwindContext db = injectedContext;

		public void OnGet()
        {
			ViewData["Title"] = "Northwind B2B - Customers";
			Customers = db.Customers.OrderBy(c => c.Country);
		}
    }
}
