using DigitalDistribution.Models.Database.Requests;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigitalDistribution.Helpers;

namespace DigitalDistribution.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<ObjectResult> GetUserDetails()
        {
            return Ok(await _userService.GetUserDetails(User.GetUserId()));
        }

        [HttpPost("register")]
        public async Task<ObjectResult> Register([FromBody] UserRegisterRequest userRequest,
            [FromQuery] string role)
        {
            return Ok(await _userService.RegisterUser(userRequest, role));
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
