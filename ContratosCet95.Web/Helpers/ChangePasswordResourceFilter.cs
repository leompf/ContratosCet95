using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContratosCet95.Web.Helpers;

public class ChangePasswordResourceFilter : IAsyncResourceFilter
{
    private readonly UserManager<User> _userManager;
    private readonly LinkGenerator _linkGenerator;

    public ChangePasswordResourceFilter(UserManager<User> userManager, LinkGenerator linkGenerator)
    {
        _userManager = userManager;
        _linkGenerator = linkGenerator;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        var httpContext = context.HttpContext;

        if (httpContext.User.Identity?.IsAuthenticated ?? false)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user != null && user.IsChangePassword)
            {
                var redirectUrl = _linkGenerator.GetPathByAction(
                    httpContext: httpContext,
                    action: "EditAccount",
                    controller: "Account"
                );

                if (!string.Equals(httpContext.Request.Path, redirectUrl, StringComparison.OrdinalIgnoreCase))
                {
                    context.Result = new RedirectResult(redirectUrl);
                    return; 
                }
            }
        }

        await next();
    }
}
