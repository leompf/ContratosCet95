using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Data.Entities;

public class TipoContrato : IEntity
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } = null!;

    [Required]
    [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 months.")]
    public int DurationMonths { get; set; } 
}
