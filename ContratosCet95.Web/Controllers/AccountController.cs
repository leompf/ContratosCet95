using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Helpers;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    #region Authentication
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
    #endregion

    #region User Creation
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

    [HttpPost]
    public async Task<IActionResult> Register(RegisterNewUserViewModel model)
    {
        model.Roles = _userHelper.GetComboUserRoles();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userHelper.GetUserByEmailAsync(model.Username);
        if (user != null)
        {
            ModelState.AddModelError(string.Empty, "A user with this email already exists.");
            return View(model);
        }

        user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Username,
            UserName = model.Username
        };

        //TODO: Gerar uma password random
        var result = await _userHelper.AddUserAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        var role = await _roleManager.FindByIdAsync(model.RoleId);

        await _userHelper.AddUserToRoleAsync(user, role.Name);

        model.StatusMessage = "User created successfully! Redirecting...";
        return View(model);
    }
    #endregion

    #region Account CRUD
    [HttpGet]
    public async Task<IActionResult> EditAccount()
    {
        var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
        if (user == null)
        {
            return NotFound();
        }

        var model = new AccountViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAccount(AccountViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userHelper.GetUserByEmailAsync(model.Email);
        if (user == null)
            return NotFound();

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;

        var result = await _userHelper.UpdateUserAsync(user);

        if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
        {
            var passwordChangeResult = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!passwordChangeResult.Succeeded)
            {
                foreach (var error in passwordChangeResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            ViewBag.PasswordMessage = "A tua palavra-passe foi alterada com sucesso!";
        }

        return View(model);
    }
    #endregion
}
