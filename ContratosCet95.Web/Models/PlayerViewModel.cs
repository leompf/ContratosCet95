using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Models;

public class PlayerViewModel
{
    public string Name { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly? Birthdate { get; set; }
}
