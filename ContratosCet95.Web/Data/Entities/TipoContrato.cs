using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Data.Entities;

public class TipoContrato : IEntity
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } = null!;
}
