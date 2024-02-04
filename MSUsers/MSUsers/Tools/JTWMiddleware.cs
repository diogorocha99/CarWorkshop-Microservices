using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MSUsers.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MSUsers.Tools
{
    public class JTWMiddleware
    {
        public class JWTMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly IConfiguration _configuration;
            //private readonly IUserService _userService;

            public JWTMiddleware(RequestDelegate next, IConfiguration configuration)
            {
                _next = next;
                _configuration = configuration;
                //_userService = userService;
            }

            public async Task Invoke(HttpContext context)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                    attachAccountToContext(context, token);

                await _next(context);
            }

            private void attachAccountToContext(HttpContext context, string token)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;

                    // attach account to context on successful jwt validation
                    //context.Items["User"] = _userService.GetUserDetails();
                }
                catch
                {
                    // do nothing if jwt validation fails
                    // account is not attached to context so request won't have access to secure routes
                }
            }

            [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
            public class AuthorizeAttribute : Attribute, IAuthorizationFilter
            {
                public void OnAuthorization(AuthorizationFilterContext context)
                {
                    var account = (AuthenticationResponse)context.HttpContext.Items["User"];
                    if (account == null)
                    {
                        // not logged in
                        context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
            }
        }
    }
}
