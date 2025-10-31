using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Models;

public class CreateContractViewModel
{
    [Required]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "You must select a Player")]
    public int JogadorId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "You must select a Team")]
    public int EquipaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "You must select a Contract Type")]
    public int TipoContratoId { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly StartDate { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly EndDate { get; set; }

    [Required]
    public string Conditions { get; set; } = null!;


    [BindNever]
    public string? StatusMessage { get; set; }

    public IEnumerable<SelectListItem>? Types { get; set; } 
    public IEnumerable<SelectListItem>? Players { get; set; } 
    public IEnumerable<SelectListItem>? Teams { get; set; } 
}
