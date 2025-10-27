﻿using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Helpers;

public class UserHelper : IUserHelper
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }


    #region CRUD User
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
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
        var list = _roleManager.Roles.Select(r => new SelectListItem
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
    #endregion
}
