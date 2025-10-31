using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Data.Entities;

public class Equipa : IEntity
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Team Name")]
    public string Name { get; set; } = null!;

    public string? City { get; set; }

    // Relação com Contratos
    public IEnumerable<Contrato> Contracts { get; set; } = new List<Contrato>();
}
