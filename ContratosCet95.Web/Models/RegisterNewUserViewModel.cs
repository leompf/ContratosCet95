using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Models;

public class RegisterNewUserViewModel
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }


    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }


    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }


    [Required]
    [MinLength(6)]
    public string Password { get; set; }


    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a role")]
    public int RoleId { get; set; }


    public IEnumerable<SelectListItem> Roles { get; set; }
}
