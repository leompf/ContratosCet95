using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContratosCet95.Web.Data;

public class ContratoRepository : GenericRepository<Contrato>, IContratoRepository
{
    private readonly DataContext _context;
    private readonly ILogger<ContratoRepository> _logger;

    public ContratoRepository(DataContext context,
        ILogger<ContratoRepository> logger) : base(context)
    {
        _context = context;
        _logger = logger;
    }

    public IEnumerable<SelectListItem> GetComboContractTypes()
    {
        var list = _context.TiposContratos
        .OrderBy(r => r.Type)
        .Select(r => new SelectListItem
        {
            Text = $"{r.Type}",
            Value = r.Id.ToString(),
        }).ToList();

        list.Insert(0, new SelectListItem
        {
            Text = "(Select a contract type...)",
            Value = "0"
        });

        return list;
    }

    public List<Contrato> FilterAndSortContracts(IEnumerable<Contrato> contracts, string? name, string? player, string? team, string? type, DateOnly? startDate, DateOnly? endDate, string? sortBy, bool sortDescending)
    {
        var query = contracts.AsEnumerable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(c => !string.IsNullOrEmpty(c.Name) &&
                                     c.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(player))
            query = query.Where(c => !string.IsNullOrEmpty(c.Jogador.FullName) &&
                                     c.Jogador.FullName.Contains(player, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(team))
            query = query.Where(c => !string.IsNullOrEmpty(c.Equipa.Name) &&
                                     c.Equipa.Name.Contains(team, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(type))
            query = query.Where(c => !string.IsNullOrEmpty(c.TipoContrato.Type) &&
                                     c.TipoContrato.Type.Contains(type, StringComparison.OrdinalIgnoreCase));

        if (startDate.HasValue)
            query = query.Where(c => c.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.EndDate <= endDate.Value);

        query = (sortBy?.ToLower()) switch
        {
            "name" => sortDescending ? query.OrderByDescending(c => c.Name)
                                     : query.OrderBy(c => c.Name),

            "player" => sortDescending ? query.OrderByDescending(c => c.Jogador.FullName)
                                       : query.OrderBy(c => c.Jogador.FullName),

            "team" => sortDescending ? query.OrderByDescending(c => c.Equipa.Name)
                                     : query.OrderBy(c => c.Equipa.Name),

            "type" => sortDescending ? query.OrderByDescending(c => c.TipoContrato.Type)
                                     : query.OrderBy(c => c.TipoContrato.Type),

            "startdate" => sortDescending ? query.OrderByDescending(c => c.StartDate)
                                          : query.OrderBy(c => c.StartDate),

            "enddate" => sortDescending ? query.OrderByDescending(c => c.EndDate)
                                        : query.OrderBy(c => c.EndDate),

            _ => sortDescending ? query.OrderByDescending(c => c.Name)
                                : query.OrderBy(c => c.Name)
        };

        var result = query.ToList();

        _logger.LogInformation("Filtered contracts count: {Count}", result.Count);
        return result;
    }

    public IQueryable<Contrato> GetAllContracts()
    {
        return _context.Contratos
             .Include(c => c.Jogador)
             .Include(c => c.Equipa)
             .Include(c => c.TipoContrato);
    }
}
