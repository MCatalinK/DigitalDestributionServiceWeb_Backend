using DigitalDistribution.Models.Database.Requests;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigitalDistribution.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DigitalDistribution.Models.Exceptions;
using DigitalDistribution.Models.Constants;
using AutoMapper;
using DigitalDistribution.Models.Database.Entities;
using System.Collections.Generic;
using DigitalDistribution.Models.Responses.Library;

namespace DigitalDistribution.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly LibraryService _libraryService;
        private readonly IMapper _mapper;

        public UserController(UserService userService,
            LibraryService libraryService,
            IMapper mapper)
        {
            _userService = userService;
            _libraryService = libraryService;
            _mapper = mapper;
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
            return Ok(await _userService.Get()
                .Include(p=>p.UserRoles)
                .ToListAsync());
        }

        [Authorize]
        [HttpGet("library")]
        public async Task<ObjectResult> GetItemsFromLibrary()
        {
            var library = await _libraryService.Get(p => p.UserId == User.GetUserId())
                .Include(p=>p.Product)
                .ToListAsync();
           

            if (library is null)
                throw new NotFoundException(StringConstants.LibraryNotFound);

            return Ok(_mapper.Map<List<LibraryResponse>>(library));
        }

        [HttpPost("register")]
        public async Task<ObjectResult> Register([FromBody] UserRegisterRequest userRequest,
            [FromQuery] string role,[FromQuery] int? devTeamId)
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
