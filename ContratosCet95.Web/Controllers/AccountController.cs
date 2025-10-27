using ContratosCet95.Web.Data;
using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Helpers;
using ContratosCet95.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator;

namespace ContratosCet95.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserHelper _userHelper;
    private readonly IJogadorRepository _jogadorRepository;
    private readonly IEmailSender _emailSender;
    private readonly Password _password;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConverterHelper _converterHelper;

    public AccountController(IUserHelper userHelper,
        IJogadorRepository jogadorRepository,
        IEmailSender emailSender,
        Password password,
        RoleManager<IdentityRole> roleManager,
        IConverterHelper converterHelper)
    {
        _userHelper = userHelper;
        _jogadorRepository = jogadorRepository;
        _emailSender = emailSender;
        _password = password;
        _roleManager = roleManager;
        _converterHelper = converterHelper;
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

    #region Account Creation
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
    [ValidateAntiForgeryToken]
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
            UserName = model.Username,
            Birthdate = model.Birthdate,
            IsChangePassword = true
        };

        var password = _password.LengthRequired(8).Next();

        var result = await _userHelper.AddUserAsync(user, password);

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

        var jogador = await _jogadorRepository.CreateJogadorAsync(user, "Jogador");
        if (jogador != null)
        {
            await _jogadorRepository.CreateAsync(jogador);
        }

        var token = await _userHelper.GenerateEmailConfirmationToken(user);
        var confirmationLink = Url.Action("ConfirmEmail", "Account", new
        {
            userId = user.Id,
            token = token
        }, protocol: HttpContext.Request.Scheme);

        var mail = $@"<p>Hello {user.FirstName},</p>
        <p>An account has been created for you in our platform. We bid you a warm welcome and hope you make great use of it! In here, you are able to view all your active or past contracts.<p>
        <p>Your temporary password is: <b>{password}</b></p>
        <p>You will be redirected to change your password the first time you login. You also must confirm your email address before attempting doing so by clicking <a href='{confirmationLink}'>here</a>.</p>
        <p>Thank you for joining us,<br />
        Best Regards<br />
        Leonardo Fraqueiro</p>";

        await _emailSender.SendEmailAsync(user.Email, "Account Creation - Contratos", mail);

        model.StatusMessage = "User created successfully! Redirecting...";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return View("ConfirmEmailFailure");
        }

        var user = await _userHelper.GetUserByIdAsync(userId);
        if (user == null)
        {
            return View("ConfirmEmailFailure");
        }

        var result = await _userHelper.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return View("ConfirmEmailSuccess");
        }

        return View("ConfirmEmailFailure");
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
            user.IsChangePassword = false;
            await _userHelper.UpdateUserAsync(user);
            ViewBag.PasswordMessage = "A tua palavra-passe foi alterada com sucesso!";
        }

        return View(model);
    }
    #endregion

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ViewAllUsers(string? name, string? email, string? role, string? sortBy, bool sortDescending = true)
    {
        var users = _userHelper.GetAllUsers();
        var userList = new List<UserViewModel>();

        foreach (var user in users)
        {
            var userRole = await _userHelper.GetUserRoleAsync(user);
            userList.Add(_converterHelper.ToUserViewModel(user, userRole));
        }

        userList = _userHelper.FilterAndSortUsers(userList, name, email, role, sortBy, sortDescending);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            var partialModel = new UserListViewModel
            {
                Users = userList
            };
            return PartialView("_ViewAllUsersTable", partialModel.Users);
        }

        var model = new UserListViewModel
        {
            NameFilter = name,
            EmailFilter = email,
            Users = userList,
            Roles = _userHelper.GetAllRoles()
        };

        ViewBag.DefaultSortColumn = sortBy ?? "Name";
        ViewBag.DefaultSortDescending = sortDescending;

        return View(model);
    }
}
