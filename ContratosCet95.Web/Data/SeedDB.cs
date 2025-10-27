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

        var user = await _userHelper.GetUserByEmailAsync("lmfraqueiro@gmail.com");

        if (user == null)
        {
            user = new User
            {
                FirstName = "Leonardo",
                LastName = "Fraqueiro",
                Email = "lmfraqueiro@gmail.com",
                UserName = "lmfraqueiro@gmail.com",
                PhoneNumber = "21111111",
                IsChangePassword = false,
                EmailConfirmed = true
            };

            var result = await _userHelper.AddUserAsync(user, "123456");

            if (result != IdentityResult.Success)
            {
                throw new InvalidOperationException("Could not create User in Seeder");
            }

            await _userHelper.AddUserToRoleAsync(user, "Admin");
        }

        var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
        if (!isInRole)
        {
            await _userHelper.AddUserToRoleAsync(user, "Admin");
        }
    }
}
