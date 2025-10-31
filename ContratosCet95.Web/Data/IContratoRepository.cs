using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Data;

public interface IContratoRepository : IGenericRepository<Contrato>
{
    IEnumerable<SelectListItem> GetComboContractTypes();
}
