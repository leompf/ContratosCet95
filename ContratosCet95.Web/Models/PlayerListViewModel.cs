using ContratosCet95.Web.Data.Entities;

namespace ContratosCet95.Web.Models;

public class PlayerListViewModel
{
    public string? NameFilter { get; set; }

    public DateOnly? BirthdateFilter { get; set; }

    public IEnumerable<Jogador> Players { get; set; }
}
