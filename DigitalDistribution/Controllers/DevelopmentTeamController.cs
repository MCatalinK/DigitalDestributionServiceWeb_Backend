using AutoMapper;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.DevelopmentTeam;
using DigitalDistribution.Models.Database.Responses.DevelopmentTeam;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [ApiController]
    [Route("api/developerTeams")]
    public class DevelopmentTeamController:ControllerBase
    {
        private readonly DevelopmentTeamService _developmentTeamService;
        private readonly IMapper _mapper;

        public DevelopmentTeamController(DevelopmentTeamService developmentTeamService,
            IMapper mapper)
        {
            _developmentTeamService = developmentTeamService;
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
    }
}
