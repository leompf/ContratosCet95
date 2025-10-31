using ContratosCet95.Web.Data;
using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContratosCet95.Web.Controllers;

public class PlayersController : Controller
{
    private readonly IJogadorRepository _jogadorRepository;

    public PlayersController(IJogadorRepository jogadorRepository)
    {
        _jogadorRepository = jogadorRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult ViewAllPlayers(string? name, DateOnly? birthdate, string? sortBy, bool sortDescending = true)
    {
        var players = _jogadorRepository.GetAll();
        var playerList = new List<Jogador>();

        playerList = _jogadorRepository.FilterAndSortPlayers(players, name, birthdate, sortBy, sortDescending);
        
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            var partialModel = new PlayerListViewModel
            {
                Players = playerList
            };
            return PartialView("_ViewAllPlayersTable", partialModel.Players);
        }

        var model = new PlayerListViewModel
        {
            NameFilter = name,
            BirthdateFilter = birthdate,
            Players = playerList
        };

        ViewBag.DefaultSortColumn = sortBy ?? "Name";
        ViewBag.DefaultSortDescending = sortDescending;

        return View(model);
    }
}
