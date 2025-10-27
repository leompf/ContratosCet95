using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace ContratosCet95.Web.Data.Entities;

public class User : IdentityUser
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;


    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;


    public bool IsChangePassword { get; set; }
}
