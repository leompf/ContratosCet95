using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Helpers;
using Microsoft.AspNetCore.Identity;

namespace ContratosCet95.Web.Data;

public class SeedDB
{
    private readonly DataContext _context;
    private readonly IUserHelper _userHelper;

    public SeedDB(DataContext context, IUserHelper userHelper)
    {
        _context = context;
        _userHelper = userHelper;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        await _userHelper.CheckRoleAsync("Admin");
        await _userHelper.CheckRoleAsync("Funcionário");
        await _userHelper.CheckRoleAsync("Jogador");

        await AddUser("Leonardo", "Fraqueiro", "lmfraqueiro@gmail.com", "Admin");
        await AddUser("Jogador", "Teste", "lmpfraqueiro@gmail.com", "Jogador");
        await AddUser("Funcionario", "Teste", "kowayil678@dropeso.com", "Funcionário");

        AddTeam("Benfica", "Lisboa");
        AddTeam("Sporting", "Lisboa");
        AddTeam("Alverca", "Alverca do Ribatejo");
        AddTeam("Porto", "Porto");
        AddTeam("Boavista", "Porto");
        AddTeam("Estoril", "Estoril");
        AddTeam("Gil Vicente", "Barcelos");
        AddTeam("Nacional", "Funchal");
        AddTeam("VitóriaSC", "Guimarães");


        if (!_context.TiposContratos.Any())
        {
            AddContractType("Trimestral");
            AddContractType("Semestral");
            AddContractType("Anual");
            AddContractType("Permanente");
            await _context.SaveChangesAsync();
        }
    }

    private void AddContractType(string type)
    {
        int durationMonths = type switch
        {
            "Trimestral" => 3,
            "Semestral" => 6,
            "Anual" => 12,
            "Permanente" => 100, 
            _ => throw new ArgumentException($"Unknown contract type: {type}")
        };

        _context.TiposContratos.Add(new TipoContrato
        {
            Type = type,
            DurationMonths = durationMonths
        });
    }

    private async Task AddUser(string firstName, string lastName, string email, string role)
    {
        var user = await _userHelper.GetUserByEmailAsync(email);

        if (user == null)
        {
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = "21111111",
                IsChangePassword = false,
                EmailConfirmed = true
            };

            var result = await _userHelper.AddUserAsync(user, "123456");

            if (result != IdentityResult.Success)
            {
                throw new InvalidOperationException("Could not create User in Seeder");
            }

            await _userHelper.AddUserToRoleAsync(user, role);

            var isInRole = await _userHelper.IsUserInRoleAsync(user, role);
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            if (role == "Jogador")
            {
                var jogador = new Jogador
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Birthdate = user.Birthdate,
                    User = user,
                };

                _context.Jogadores.Add(jogador);
                await _context.SaveChangesAsync();
            }
        }
    }

    private void AddTeam(string name, string city)
    {
        _context.Equipas.Add(new Equipa
        {
            Name = name,
            City = city
        });            
    }
}
