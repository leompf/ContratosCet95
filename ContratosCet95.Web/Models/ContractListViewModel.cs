using ContratosCet95.Web.Data.Entities;

namespace ContratosCet95.Web.Models;

public class ContractListViewModel
{
    public string? NameFilter { get; set; }

    public string? PlayerFilter { get; set; }

    public string? TeamFilter { get; set; }

    public string? TypeFilter { get; set; }

    public DateOnly? StartDateFilter {  get; set; }

    public DateOnly? EndDateFilter {  get; set; }

    public IEnumerable<Contrato> Contracts { get; set; }
}
