using DigitalDistribution.Models.Database.Requests;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigitalDistribution.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DigitalDistribution.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ObjectResult> GetUserDetails()
        {
            return Ok(await _userService.GetUserDetails(User.GetUserId()));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/")]
        public async Task<ObjectResult> GetAllUsers()
        {
            return Ok(await _userService.Get().ToListAsync());
        }


        [HttpPost("register")]
        public async Task<ObjectResult> Register([FromBody] UserRegisterRequest userRequest,
            [FromQuery] string role,[FromRoute] int? devTeamId)
        {
            return Ok(await _userService.RegisterUser(userRequest, role,devTeamId));
        }

        [HttpPost("login")]
        public async Task<ObjectResult> Login([FromBody] UserLoginRequest userRequest)
        {
            return Ok(await _userService.Login(userRequest.Username, userRequest.Password));
        }

        [HttpPut("token/refresh")]
        public async Task<ObjectResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            return Ok(await _userService.RefreshToken(refreshTokenRequest.RefreshToken));
        }

        [HttpPut("token/revoke")]
        public async Task<ObjectResult> RevokeToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            return Ok(await _userService.RevokeRefreshToken(refreshTokenRequest.RefreshToken));
        }
       
    }
}
