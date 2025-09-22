using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace ContratosCet95.Web.Data.Entities;

public class User : IdentityUser
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }


    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
}
