using ContratosCet95.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace ContratosCet95.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly DataContext _context;

        public ContractsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("get-end-date")]
        public IActionResult GetEndDate(int typeId, DateTime startDate)
        {
            var type = _context.TiposContratos.FirstOrDefault(t => t.Id == typeId);
            if (type == null)
                return NotFound();

            var endDate = startDate.AddMonths(type.DurationMonths);

            return Ok(new { endDate = endDate.ToString("yyyy-MM-dd") });
        }
    }
}
