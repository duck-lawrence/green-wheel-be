using Application.Abstractions;
using Application.Dtos.User.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        //private readonly IGoogleCredentialService _googleService;

        public UserController(IUserService service
            //,IGoogleCredentialService googleCredentialService
            )
        {
            _userService = service;
            //_googleService = googleCredentialService;

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReq user)
        {
            var accessToken = await _userService.Login(user);
            return Ok(new
            {
                AccessToken = accessToken
            });
        }
    }
}
