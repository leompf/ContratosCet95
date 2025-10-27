using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Helpers;

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
    }
}
