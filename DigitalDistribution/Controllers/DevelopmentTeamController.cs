using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.Models.Constants;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests.DevelopmentTeam;
using DigitalDistribution.Models.Exceptions;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/devTeams")]
    public class DevelopmentTeamController:ControllerBase
    {
        private readonly BaseService<DevelopmentTeamEntity> _developmentTeamService;
        private readonly IMapper _mapper;

        public DevelopmentTeamController(BaseService<DevelopmentTeamEntity> developmentTeamService,
            IMapper mapper)
        {
            _developmentTeamService = developmentTeamService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAllDevelopmentTeams()
        {
            var result = await _developmentTeamService.Get()
                .Include(p=>p.Products)
                .ToListAsync();

            if (result is null)
                throw new NotFoundException(StringConstants.NoDevTeams);

            return Ok(result);
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<ObjectResult> AddDevelopmentTeam([FromBody] DevelopmentTeamEntity team)
        {
            var devTeam = await _developmentTeamService.Get(p=>p.Name==team.Name).FirstOrDefaultAsync();
            if (devTeam is null)
                return Ok(await _developmentTeamService.Create(team));
            throw new ItemExistsException(StringConstants.DevTeamExists); 
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{teamId}")]
        public async Task<ObjectResult> DeleteDevelopmentTeam([FromRoute] int teamId)
        {
            var devTeam = await _developmentTeamService.Get(p=>p.Id==teamId).FirstOrDefaultAsync();
            if (devTeam is null)
                throw new NotFoundException(StringConstants.NoDevTeamFound);

            return Ok(await _developmentTeamService.Delete(devTeam));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{teamId}")]
        public async Task<ObjectResult> UpdateDevelopmentTeam([FromRoute] int teamId, UpdateDevTeamRequest team)
        {
            var devTeam = await _developmentTeamService.Get(p => p.Id == teamId)
                .Include(p=>p.Products)
                .FirstOrDefaultAsync();

            if (devTeam is null)
                throw new NotFoundException(StringConstants.NoDevTeamFound);

            return Ok(await _developmentTeamService.Update(_mapper.Map(team, devTeam)));
        }
    }
}
