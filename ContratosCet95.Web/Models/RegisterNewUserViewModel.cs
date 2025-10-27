using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Models;

public class RegisterNewUserViewModel
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;


    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;


    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateTime Birthdate { get; set; }


    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; } = null!;


    [Required]
    [RegularExpression("^(?!0$).*", ErrorMessage = "You must select a role")]
    public string RoleId { get; set; } = null!;


    public IEnumerable<SelectListItem>? Roles { get; set; }


    [BindNever]
    public string? StatusMessage { get; set; }
}
