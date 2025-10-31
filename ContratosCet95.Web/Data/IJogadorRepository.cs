using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Data;

public interface IJogadorRepository : IGenericRepository<Jogador>
{
    IQueryable<Jogador> GetAllPlayers();
    Task<Jogador> CreateJogadorAsync(User user, string role);
    List<Jogador> FilterAndSortPlayers(IEnumerable<Jogador> players, string? name, DateOnly? birthdate, string? sortBy, bool sortDescending);
    IEnumerable<SelectListItem> GetComboPlayers();
}
