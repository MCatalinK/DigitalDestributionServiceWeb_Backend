using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalDistribution.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserEntityController : ControllerBase
    {
        private readonly UserEntityRepository _userEntityRepository;

        public UserEntityController(UserEntityRepository userEntityRepository)
        {
            _userEntityRepository = userEntityRepository;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAll()
        {
            return Ok(await _userEntityRepository.GetAll());
        }
        [HttpGet("{username}")]
        public async Task<ObjectResult> GetById([FromRoute] string username)
        {
            return Ok(await _userEntityRepository.GetByUsername(username));
        }
        [HttpPut]
        public async Task<ObjectResult> Update([FromBody] UserEntity user)
        {
            return Ok(await _userEntityRepository.Update(user));
        }

        [HttpDelete("{username}")]
        public async Task<ObjectResult> DeleteUser([FromRoute] string username)
        {
            return Ok(await _userEntityRepository.Delete(username));
        }
        [HttpPost]
        public async Task<ObjectResult> CreateUser([FromBody] UserEntity user)
        {
            return Ok(await _userEntityRepository.Insert(user));
        }
    }
}
