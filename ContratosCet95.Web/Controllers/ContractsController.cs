using ContratosCet95.Web.Data;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContratosCet95.Web.Controllers;

public class ContractsController : Controller
{
    private readonly IEquipaRepository _equipaRepository;
    private readonly IJogadorRepository _jogadorRepository;
    private readonly IContratoRepository _contratoRepository;

    public ContractsController(IEquipaRepository equipaRepository,
        IJogadorRepository jogadorRepository,
        IContratoRepository contratoRepository)
    {
        _equipaRepository = equipaRepository;
        _jogadorRepository = jogadorRepository;
        _contratoRepository = contratoRepository;
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
}
