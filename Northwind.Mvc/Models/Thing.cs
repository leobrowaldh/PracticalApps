using System.ComponentModel.DataAnnotations;

namespace Northwind.Mvc.Models
{
    //Data anotations are da bomb!
    public record Thing(
        [Range(1, 10)] int? Id, 
        [Required] string? Color,
        [EmailAddress] string? Email
        );
}
