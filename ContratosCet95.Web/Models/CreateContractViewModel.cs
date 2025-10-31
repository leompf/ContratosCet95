using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Models;

public class CreateContractViewModel
{
    [Required]
    public string Name { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "You must select a Player")]
    public int PlayerId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "You must select a Team")]
    public int TeamId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "You must select a Contract Type")]
    public int TypeId { get; set; } 

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly StartDate { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly EndDate { get; set; }

    [Required]
    public string Conditions { get; set; } = null!;

    public IEnumerable<SelectListItem> Types { get; set; } = null!;
    public IEnumerable<SelectListItem> Players { get; set; } = null!;
    public IEnumerable<SelectListItem> Teams { get; set; } = null!;
}
