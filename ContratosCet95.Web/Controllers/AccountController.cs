using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Helpers;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContratosCet95.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserHelper _userHelper;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(IUserHelper userHelper, RoleManager<IdentityRole> roleManager)
    {
        _userHelper = userHelper;
        _roleManager = roleManager;
    }


    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userHelper.LoginAsync(model);
            if (result.Succeeded)
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        this.ModelState.AddModelError(string.Empty, "Failed to login");
        return View(model);
    }


    public async Task<IActionResult> Logout()
    {
        await _userHelper.LogoutAsync();
        return RedirectToAction("Login", "Account");
    }


    [Authorize(Roles = "Admin")]
    public IActionResult ManageUsers()
    {
        return View();
    }


    [Authorize(Roles = "Admin")]
    public IActionResult Register()
    {
        var model = new RegisterNewUserViewModel
        {
            Roles = _userHelper.GetComboUserRoles()
        };

        return View(model);
    }

    //TODO: Corrigir bug da combobox não identificar item selecionado
    [HttpPost]
    public async Task<IActionResult> Register(RegisterNewUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);

            if (user == null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Username,
                    UserName = model.Username,
                };

                var result = await _userHelper.AddUserAsync(user, model.Password);                   

                await _userHelper.AddUserToRoleAsync(user, model.RoleId.ToString());

                if (result != IdentityResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "The user could not be created.");
                    return View(model);
                }
            }
        }

        return View(model);
    }
}
