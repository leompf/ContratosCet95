using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContratosCet95.Web.Data;

public class DataContext : IdentityDbContext<User>
{
    public DbSet<Jogador> Jogadores { get; set; }
    public DbSet<Equipa> Equipas { get; set; }
    public DbSet<Contrato> Contratos { get; set; }
    public DbSet<TipoContrato> TiposContratos { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {           
    }
}
