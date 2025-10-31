using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Models;

public class CreateTeamViewModel
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Team Name")]
    public string Name { get; set; } = null!;

    public string? City { get; set; }

    [BindNever]
    public string? StatusMessage { get; set; }
}
