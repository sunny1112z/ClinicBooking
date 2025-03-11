using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var roleIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "RoleId");

            if (roleIdClaim != null)
            {
                var claimsIdentity = new ClaimsIdentity(new[] { new Claim("RoleId", roleIdClaim.Value) }, "jwt");
                context.User = new ClaimsPrincipal(claimsIdentity);
            }
        }

        await _next(context);
    }
}
