using ContratosCet95.Web.Data;
using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContratosCet95.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly DataContext _context;
        private readonly IEquipaRepository _equipaRepository;

        public TeamsController(DataContext context,
            IEquipaRepository equipaRepository)
        {
            _context = context;
            _equipaRepository = equipaRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var equipa = _equipaRepository.CreateEquipa(model);
            if (equipa != null)
            {
                await _equipaRepository.CreateAsync(equipa);
            }

            model.StatusMessage = "Team successfully created! Redirecting...";
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _equipaRepository.GetByIdAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _equipaRepository.GetByIdAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            var model = new CreateTeamViewModel
            {
                Name = team.Name,
                City = team.City
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CreateTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Id == null)
            {
                return NotFound();
            }

            var team = await _equipaRepository.GetByIdAsync(model.Id.Value);

            team.Name = model.Name;
            team.City = model.City;
            model.StatusMessage = "Team sucessfully updated! Redirecting...";

            _context.Update(team);
            await _context.SaveChangesAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _equipaRepository.GetByIdAsync(id.Value);
            if (team == null)
            {
                return NotFound();
            }

            _context.Equipas.Remove(team);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewAllTeams));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ViewAllTeams(string? name, string? city, string? sortBy, bool sortDescending = true)
        {
            var teams = _equipaRepository.GetAllTeams();
            var teamList = new List<Equipa>();

            teamList = _equipaRepository.FilterAndSortTeams(teams, name, city, sortBy, sortDescending);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var partialModel = new TeamListViewModel
                {
                    Teams = teamList
                };
                return PartialView("_ViewAllTeamsTable", partialModel.Teams);
            }

            var model = new TeamListViewModel
            {
                NameFilter = name,
                CityFilter = city,
                Teams = teamList
            };

            ViewBag.DefaultSortColumn = sortBy ?? "Name";
            ViewBag.DefaultSortDescending = sortDescending;

            return View(model);
        }
    }
}
