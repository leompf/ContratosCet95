using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Data
{
    public class JogadorRepository : GenericRepository<Jogador>, IJogadorRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ILogger<JogadorRepository> _logger;

        public JogadorRepository(DataContext context,
            IUserHelper userHelper,
            ILogger<JogadorRepository> logger) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _logger = logger;
        }

        public IQueryable<Jogador> GetAllPlayers()
        {
            var players = _context.Jogadores
                .OrderBy(e => e.FirstName)
                .AsQueryable();

            return players;
        }

        public async Task<Jogador> CreateJogadorAsync(User user, string role)
        {
            var isInRole = await _userHelper.IsUserInRoleAsync(user, role);

            if (!isInRole)
            {
                _logger.LogWarning("User {Email} is not in role {Role}, cannot create Jogador.", user.Email, role);
                return null;
            }

            var jogador = new Jogador
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                User = user
            };

            _logger.LogInformation("Jogador entity created for user {Email}.", user.Email);
            return jogador;
        }

        public List<Jogador> FilterAndSortPlayers(IEnumerable<Jogador> players, string? name, DateOnly? birthdate, string? sortBy, bool sortDescending)
        {
            _logger.LogInformation("Filtering players with Name='{Name}', Birthdate='{Brithdate}'",
            name, birthdate);

            var filtered = players.ToList();

            if (!string.IsNullOrEmpty(name))
                filtered = filtered
                    .Where(p => !string.IsNullOrEmpty(p.FullName) &&
                                p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();


            filtered = (sortBy?.ToLower()) switch
            {
                "name" => sortDescending ? filtered.OrderByDescending(p => p.FullName).ToList()
                                         : filtered.OrderBy(p => p.FullName).ToList(),                
                _ => sortDescending ? filtered.OrderByDescending(p => p.FullName).ToList()
                                    : filtered.OrderBy(p => p.FullName).ToList()
            };

            _logger.LogInformation("Filtered players count: {Count}", filtered.Count);
            return filtered;
        }

        public IEnumerable<SelectListItem> GetComboPlayers()
        {
            var list = _context.Jogadores
            .OrderBy(r => r.FirstName)
            .Select(r => new SelectListItem
            {
                Text = $"{r.FullName}",
                Value = r.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a player...)",
                Value = "0"
            });

            return list;
        }
    }
}
