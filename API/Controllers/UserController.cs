using Application.Shared;
using Application.User;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;            
        }

        [HttpPost("Login/{username}")]
        public async Task<Response<string>> Login(string userName)
        {
            return await _userService.Login(userName);
        }
    }
}
