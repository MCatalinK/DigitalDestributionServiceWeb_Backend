﻿using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Constants;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.Profile;
using DigitalDistribution.Models.Database.Responses.Profile;
using DigitalDistribution.Models.Exceptions;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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


        [HttpGet]
        public async Task<ObjectResult> GetAllProfiles()
        {
            var profiles = await _profileService.Get()
                .ToListAsync();

            if (profiles is null)
                throw new NotFoundException(StringConstants.ProfileNotFound);

            return Ok(_mapper.Map<List<ProfileDetailsResponse>>(profiles));
        }

        [HttpGet("{name}")]
        public async Task<ObjectResult> GetProfilesByName([FromRoute] string name)
        {
            var profiles = await _profileService.Search(name);
            if (profiles is null)
                throw new NotFoundException(StringConstants.ProfileNotFound);

            return Ok(_mapper.Map<List<ProfileDetailsResponse>>(profiles));
        }

        [HttpPost]
        public async Task<ObjectResult> AddProfile([FromBody] ProfileEntity profile)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
               .Include(p => p.Profile)
               .FirstOrDefaultAsync();

            if (normalUser?.Profile is null)
            {
                profile.UserId = normalUser.Id;
                return Ok(await _profileService.Create(profile));
            }
            throw new ItemExistsException(StringConstants.ProfileExists);
        }
   
        [HttpPut("update")]
        public async Task<ObjectResult> UpdateProfile([FromBody] UpdateProfileRequest profile)
        {
            var normalUser = await _userService.Get(p => p.Id == User.GetUserId())
                .Include(p => p.Profile)
                .FirstOrDefaultAsync();

            if (normalUser?.Profile == null)
                throw new NotFoundException(StringConstants.ProfileNotFound);

            return Ok(await _profileService.Update(_mapper.Map(profile, normalUser.Profile)));
        }
    }
}

