using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Data;

public interface IEquipaRepository : IGenericRepository<Equipa>
{
    IQueryable<Equipa> GetAllTeams();
    Equipa CreateEquipa(CreateTeamViewModel model);
    List<Equipa> FilterAndSortTeams(IEnumerable<Equipa> teams, string? name, string? city, string? sortBy, bool sortDescending);
    IEnumerable<SelectListItem> GetComboTeams();
}
