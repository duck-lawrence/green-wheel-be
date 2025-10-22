using Application.Abstractions;
using Application.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.AppExceptions;

namespace API.Filters
{
    public class RoleAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var _cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
            var user = context.HttpContext.User;
            //check user login or not?
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
            }
            //take userId
            var userId = user.FindFirstValue(JwtRegisteredClaimNames.Sid)!.ToString();
            if (userId == null)
            {
                throw new UnauthorizedAccessException(Message.UserMessage.Unauthorized);
            }
            var userService = context.HttpContext.RequestServices.GetService<IUserService>();
            if (userService == null)
            {
                throw new Exception();
            }
            var roleList = _cache.Get<List<Role>>("AllRoles");
            var userInDB = await userService.GetByIdAsync(Guid.Parse(userId));
            var userRole = roleList.FirstOrDefault(r => r.Id == userInDB.Role.Id).Name;

            if (userRole == null || !_roles.Contains(userRole))
            {
                throw new ForbidenException(Message.UserMessage.DoNotHavePermission);
            }
        }
    }
}