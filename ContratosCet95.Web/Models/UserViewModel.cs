using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Models;

public class UserViewModel
{
    public string Id { get; set; }

    public string Name { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly? Birthdate { get; set; }

    public string Email { get; set; }


    [Display(Name = "Perfil")]
    public string Role { get; set; }

    [Display(Name = "Contacto")]
    public string PhoneNumber { get; set; }
}
