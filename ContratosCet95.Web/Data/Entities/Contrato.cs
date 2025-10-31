using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Data.Entities;

public class Contrato : IEntity
{
    public int Id { get; set; }

    [Required]
    public int PlayerId { get; set; }
    public Jogador Jogador { get; set; } = null!;

    [Required]
    public int TeamId { get; set; }
    public Equipa Equipa { get; set; } = null!;

    [Required]
    public TipoContrato Type { get; set; } = null!;

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly StartDate { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly EndDate { get; set; }

    [Required]
    public string Conditions { get; set; } = null!;
}
