using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Data;

public interface IContratoRepository : IGenericRepository<Contrato>
{
    IQueryable<Contrato> GetAllContracts();
    IEnumerable<SelectListItem> GetComboContractTypes();
    List<Contrato> FilterAndSortContracts(IEnumerable<Contrato> contracts, string? name, string? player, string? team, string? type, DateOnly? startDate, DateOnly? endDate, string? sortBy, bool sortDescending);
}
