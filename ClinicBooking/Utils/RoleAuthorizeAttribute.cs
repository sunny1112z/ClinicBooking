using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly int[] _allowedRoles;

    public RoleAuthorizeAttribute(params int[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var roleIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId");

        if (roleIdClaim == null || !_allowedRoles.Contains(int.Parse(roleIdClaim.Value)))
        {
            context.Result = new ForbidResult();
        }
    }
}
