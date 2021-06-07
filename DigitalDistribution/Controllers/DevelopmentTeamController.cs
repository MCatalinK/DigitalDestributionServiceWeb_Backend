using AutoMapper;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.DevelopmentTeam;
using DigitalDistribution.Models.Database.Responses.DevelopmentTeam;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/developerTeams")]
    public class DevelopmentTeamController:ControllerBase
    {
        private readonly DevelopmentTeamService _developmentTeamService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public DevelopmentTeamController(DevelopmentTeamService developmentTeamService,
            UserService userService,
            IMapper mapper)
        {
            _developmentTeamService = developmentTeamService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAllDevelopmentTeams()
        {
            var result = await _developmentTeamService.Get().ToListAsync();
            if (result is null)
                return Ok(null);

            return Ok(_mapper.Map<DevelopmentTeamResponse>(result));
        }

        [HttpPost]
        public async Task<ObjectResult> AddDevelopmentTeam([FromBody] DevelopmentTeamEntity team)
        {
            return Ok(await _developmentTeamService.Create(team));
        }

        [HttpDelete("{teamId}")]
        public async Task<ObjectResult> DeleteDevelopmentTeam([FromRoute] int teamId)
        {
            var devTeam = await _developmentTeamService.Get(p=>p.Id==teamId).FirstOrDefaultAsync();
            if (devTeam is null)
                return Ok(null);
            return Ok(await _developmentTeamService.Delete(devTeam));
        }

        [HttpPut("update/{teamId}")]
        public async Task<ObjectResult> UpdateDevelopmentTeam([FromRoute] int teamId, UpdateDevTeamRequest team)
        {
            var devTeam = await _developmentTeamService.Get(p => p.Id == teamId).FirstOrDefaultAsync();
            if (devTeam is null)
                return Ok(null);

            return Ok(await _developmentTeamService.Update(_mapper.Map(team, devTeam)));
        }

        [HttpPost("member/{devteamId}{userId)")]
        public async Task<ObjectResult> AddNewMember([FromRoute] int devTeamId,[FromRoute] int userId)
        {
            var normalUser = await _userService.Get(p => p.Id == userId).FirstOrDefaultAsync();
            var devTeam = await _developmentTeamService.Get(p => p.Id == devTeamId).FirstOrDefaultAsync();
            if (normalUser is null)
                return Ok(null);
            if (devTeam is null)
                return Ok(null);//exception
            if (normalUser?.DevTeamId != null)
                return Ok(null);//exception
            normalUser.DevTeamId = devTeamId;
            devTeam.Users.Add(normalUser);
            return Ok(true);
        }
    }
}
