using Application.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Filters
{
    public class RoleAuthorizeFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;
        private readonly IMemoryCache _cache;

        public RoleAuthorizeFilter(IMemoryCache cache, params string[] roles )
        {
            _roles = roles;
            _cache = cache;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            //check user login or not?
            if(!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            //take userId
            var userId = user.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString();
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var userService = context.HttpContext.RequestServices.GetService<IUserService>();
            if (userService == null)
            {
                context.Result = new StatusCodeResult(500);
                return;
            }
            var roleList = _cache.Get<List<Role>>("AllRoles");
            //var userInDB = userService.

        }
    }
}
