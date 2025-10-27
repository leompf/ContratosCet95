using ContratosCet95.Web.Data;
using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContratosCet95.Web.Helpers;

public class UserHelper : IUserHelper
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserHelper> _logger;

    public UserHelper(DataContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UserHelper> logger)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }


    #region CRUD User
    public IQueryable<User> GetAllUsers()
    {
        var users = _context.Users
            .OrderBy(u => u.FirstName)
            .AsQueryable();

        return users;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<IdentityResult> AddUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
    #endregion

    #region CRUD Role
    public IEnumerable<SelectListItem> GetAllRoles()
    {
        _logger.LogInformation("Fetching all roles from the database.");

        try
        {
            var roles = _roleManager.Roles.ToList();
            _logger.LogInformation("Successfully retrieved {RoleCount} roles.", roles.Count);

            return roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching roles.");
            throw;
        }
    }

    public async Task AddUserToRoleAsync(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task CheckRoleAsync(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = roleName
            });
        }
    }

    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<string> GetUserRoleAsync(User user)
    {
        _logger.LogInformation("Fetching roles for user {UserId} ({Email})", user.Id, user.Email);

        try
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role != null)
                _logger.LogInformation("User {UserId} is in role {RoleName}", user.Id, role);
            else
                _logger.LogWarning("User {UserId} does not have any roles assigned", user.Id);

            return role!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while fetching roles for user {UserId}", user.Id);
            throw;
        }
    }
    #endregion

    #region Authentication
    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await _signInManager.PasswordSignInAsync(
            model.Username,
            model.Password,
            model.RememberMe,
           false
        );
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    #endregion

    #region Helper Methods
    public IEnumerable<SelectListItem> GetComboUserRoles()
    {
        var list = _roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id,
            }).ToList();

        list.Insert(0, new SelectListItem
        {
            Text = "(Select a role...)",
            Value = "0"
        });

        return list;
    }

    public async Task<string> GenerateEmailConfirmationToken(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    {
        _logger.LogInformation("Confirming email for user {UserId} ({Email})", user.Id, user.Email);

        try
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                _logger.LogInformation("Email confirmed successfully for user {UserId}", user.Id);
            else
                _logger.LogWarning("Email confirmation failed for user {UserId}. Errors: {Errors}",
                    user.Id, string.Join(", ", result.Errors.Select(e => e.Description)));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while confirming email for user {UserId}", user.Id);
            throw;
        }
    }

    public List<UserViewModel> FilterAndSortUsers(IEnumerable<UserViewModel> users, string? name, string? email, string? role, string? sortBy, bool sortDescending)
    {
        _logger.LogInformation("Filtering users with Name='{Name}', Email='{Email}'",
        name, email);

        var filtered = users.ToList();

        if (!string.IsNullOrEmpty(name))
            filtered = filtered
                .Where(u => !string.IsNullOrEmpty(u.Name) &&
                            u.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!string.IsNullOrEmpty(email))
            filtered = filtered
                .Where(u => !string.IsNullOrEmpty(u.Email) &&
                            u.Email.Contains(email, StringComparison.OrdinalIgnoreCase))
                .ToList();

        filtered = (sortBy?.ToLower()) switch
        {
            "name" => sortDescending ? filtered.OrderByDescending(u => u.Name).ToList()
                                     : filtered.OrderBy(u => u.Name).ToList(),
            "email" => sortDescending ? filtered.OrderByDescending(u => u.Email).ToList()
                                      : filtered.OrderBy(u => u.Email).ToList(),
            "role" => sortDescending ? filtered.OrderByDescending(u => u.Role).ToList()
                                     : filtered.OrderBy(u => u.Role).ToList(),
            _ => sortDescending ? filtered.OrderByDescending(u => u.Name).ToList()
                                : filtered.OrderBy(u => u.Name).ToList()
        };

        _logger.LogInformation("Filtered users count: {Count}", filtered.Count);
        return filtered;
    }
    #endregion
}
