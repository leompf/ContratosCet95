using ContratosCet95.Web.Data.Entities;

namespace ContratosCet95.Web.Models;

public class TeamListViewModel
{
    public string? NameFilter { get; set; }

    public string? CityFilter { get; set; }

    public IEnumerable<Equipa> Teams { get; set; }
}
