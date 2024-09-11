using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Packt.Shared;

namespace Northwind.Web.Pages
{
    public class CustomerInformationModel(NorthwindContext injectedContext) : PageModel
    {
		private readonly NorthwindContext db = injectedContext;
		[BindProperty(SupportsGet = true)]
		public string Id { get; set; }
		public Customer? CustomerToDisplay { get; set; }

		public void OnGet()
        {
			ViewData["Title"] = "Northwind B2B - Customer Information";
			CustomerToDisplay = db.Customers.Include(c => c.Orders).FirstOrDefault(c => c.CustomerId == Id);
		}
    }
}
