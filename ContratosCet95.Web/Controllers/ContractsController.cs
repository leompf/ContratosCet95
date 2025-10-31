using ContratosCet95.Web.Data;
using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Helpers;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ContratosCet95.Web.Controllers;

public class ContractsController : Controller
{
    private readonly IEquipaRepository _equipaRepository;
    private readonly IJogadorRepository _jogadorRepository;
    private readonly IContratoRepository _contratoRepository;
    private readonly IConverterHelper _converterHelper;

    public ContractsController(IEquipaRepository equipaRepository,
        IJogadorRepository jogadorRepository,
        IContratoRepository contratoRepository,
        IConverterHelper converterHelper)
    {
        _equipaRepository = equipaRepository;
        _jogadorRepository = jogadorRepository;
        _contratoRepository = contratoRepository;
        _converterHelper = converterHelper;
    }

    [HttpGet]
    [Authorize(Roles = "Funcionário")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Funcionário")]
    public IActionResult Create()
    {
        var model = new CreateContractViewModel
        {
            Players = _jogadorRepository.GetComboPlayers(),
            Teams = _equipaRepository.GetComboTeams(),
            Types = _contratoRepository.GetComboContractTypes()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateContractViewModel model)
    {
        if (ModelState.IsValid)
        {
            var contract = _converterHelper.ToContrato(model);

            await _contratoRepository.CreateAsync(contract);

            model.StatusMessage = "Contract successfully created! Redirecting...";
            return View(model);
        } 
        
        
        return View(model);
    }

    public IActionResult ViewAllContracts(string name, string? player, string? team, string? type, DateOnly? startDate, DateOnly? endDate, string? sortBy, bool sortDescending)
    {
        var contracts = _contratoRepository.GetAllContracts();
        var contractsList = new List<Contrato>();

        contractsList = _contratoRepository.FilterAndSortContracts(contracts, name, player, team, type, startDate, endDate, sortBy, sortDescending);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            var partialModel = new ContractListViewModel
            {
                Contracts = contractsList
            };
            return PartialView("_ViewAllContractsTable", partialModel.Contracts);
        }

        var model = new ContractListViewModel
        {
            NameFilter = name,
            PlayerFilter = player,
            TeamFilter = team,
            TypeFilter = type,
            StartDateFilter = startDate,
            EndDateFilter = endDate,
            Contracts = contractsList
        };

        ViewBag.DefaultSortColumn = sortBy ?? "Name";
        ViewBag.DefaultSortDescending = sortDescending;

        return View(model);
    }
}
