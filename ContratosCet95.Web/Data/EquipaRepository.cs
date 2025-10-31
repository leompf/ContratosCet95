using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Data;

public class EquipaRepository : GenericRepository<Equipa>, IEquipaRepository
{
    private readonly DataContext _context;
    private readonly ILogger<EquipaRepository> _logger;

    public EquipaRepository(DataContext context,
        ILogger<EquipaRepository> logger) : base(context)
    {
        _context = context;
        _logger = logger;
    }

    public IQueryable<Equipa> GetAllTeams()
    {
        var teams = _context.Equipas
            .OrderBy(e => e.Name)
            .AsQueryable();

        return teams;
    }

    public Equipa CreateEquipa(CreateTeamViewModel model)
    {
        return new Equipa
        {
            Name = model.Name,
            City = model.City
        };
    }

    public List<Equipa> FilterAndSortTeams(IEnumerable<Equipa> teams, string? name, string? city, string? sortBy, bool sortDescending)
    {
        _logger.LogInformation("Filtering teams with Name='{Name}', City='{City}'",
       name, city);

        var filtered = teams.ToList();

        if (!string.IsNullOrEmpty(name))
            filtered = filtered
                .Where(t => !string.IsNullOrEmpty(t.Name) &&
                            t.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!string.IsNullOrEmpty(city))
            filtered = filtered
                .Where(t => !string.IsNullOrEmpty(t.City) &&
                            t.City.Contains(city, StringComparison.OrdinalIgnoreCase))
                .ToList();

        filtered = (sortBy?.ToLower()) switch
        {
            "name" => sortDescending ? filtered.OrderByDescending(t => t.Name).ToList()
                                     : filtered.OrderBy(t => t.Name).ToList(),
            "city" => sortDescending ? filtered.OrderByDescending(t => t.City).ToList()
                                      : filtered.OrderBy(t => t.City).ToList(),
            _ => sortDescending ? filtered.OrderByDescending(t => t.Name).ToList()
                                : filtered.OrderBy(t => t.Name).ToList()
        };

        _logger.LogInformation("Filtered teams count: {Count}", filtered.Count);
        return filtered;
    }

    public IEnumerable<SelectListItem> GetComboTeams()
    {
        var list = _context.Equipas
            .OrderBy(r => r.Name)
            .Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString(),
            }).ToList();

        list.Insert(0, new SelectListItem
        {
            Text = "(Select a team...)",
            Value = "0"
        });

        return list;
    }
}
