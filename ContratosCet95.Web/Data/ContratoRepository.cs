using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Data;

public class ContratoRepository : GenericRepository<Contrato>, IContratoRepository
{
    private readonly DataContext _context;

    public ContratoRepository(DataContext context) : base(context)
    {
        _context = context;
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
}
