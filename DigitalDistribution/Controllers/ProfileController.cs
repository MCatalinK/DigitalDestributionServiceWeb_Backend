﻿using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Profile;
using DigitalDistribution.Models.Database.Responses.Profile;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/profile")]
    public class ProfileController:ControllerBase
    {
        private readonly ProfileService _profileService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public ProfileController(ProfileService profileService,
            UserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _profileService = profileService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ObjectResult> AddProfile([FromBody] ProfileEntity profile)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Profile)
               .FirstOrDefaultAsync();

            if (normalUser is null)
                return Ok(null);

            if (normalUser?.Profile is null)
            {
                profile.UserId = normalUser.Id;
                return Ok(await _profileService.Create(profile));
            }
            return Ok(null);//exception
        }

        [HttpGet]
        public async Task<ObjectResult> GetProfiles()
        {
            var normalUser = await _userService.Get()
                .Include(p=>p.Profile)
                .FirstOrDefaultAsync();

            if (normalUser is null)
                return Ok(null);
            return Ok(_mapper.Map<ProfileDetailsResponse>(normalUser.Profile));
        }
   
        [HttpPut("update")]
        public async Task<ObjectResult> UpdateProfile([FromBody] UpdateProfileRequest profile)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Profile)
                .FirstOrDefaultAsync();

            if (normalUser?.Profile == null)
                return Ok(null);

            return Ok(await _profileService.Update(_mapper.Map(profile, normalUser.Profile)));
        }
    }
}

