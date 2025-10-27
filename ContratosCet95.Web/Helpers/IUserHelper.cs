using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Helpers;

public interface IUserHelper
{
    IQueryable<User> GetAllUsers();
    Task<User> GetUserByEmailAsync(string email);

    Task<IdentityResult> AddUserAsync(User user, string password);

    Task<IdentityResult> UpdateUserAsync(User user);

    Task<SignInResult> LoginAsync(LoginViewModel model);

    Task LogoutAsync();

    Task CheckRoleAsync(string roleName);

    Task AddUserToRoleAsync(User user, string roleName);

    Task<bool> IsUserInRoleAsync(User user, string roleName);

    Task<string> GetUserRoleAsync(User user);

    IEnumerable<SelectListItem> GetComboUserRoles();

    IEnumerable<SelectListItem> GetAllRoles();

    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

    Task<string> GenerateEmailConfirmationToken(User user);

    Task<IdentityResult> ConfirmEmailAsync(User user, string token);

    Task<User> GetUserByIdAsync(string userId);

    List<UserViewModel> FilterAndSortUsers(IEnumerable<UserViewModel> users, string? name, string? email, string? role, string? sortBy, bool sortDescending);
}
