using Packt.Shared;

namespace Northwind.Mvc.Models
{
	//this is a good naming convention for view models:
	//and record is good here because views should be immutable.
	public record HomeIndexViewModel
	(
		int VisitorCount,
		IList<Category> Categories,
		IList<Product> Products
	);
}
