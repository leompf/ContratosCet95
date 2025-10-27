using ContratosCet95.Web.Data.Entities;

namespace ContratosCet95.Web.Data;

public interface IJogadorRepository : IGenericRepository<Jogador>
{
    Task<Jogador> CreateJogadorAsync(User user, string role);
}
