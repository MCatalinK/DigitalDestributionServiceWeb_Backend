using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Requests;
using DigitalDistribution.Models.Database.Responses;
using DigitalDistribution.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDistribution.Services
{
    public class UserService
    {

        private readonly UserRepository _userRepository;

        private readonly string _tokenKey = @"asdilasjdlnsac213kmopfa-2asda@";

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IQueryable<UserEntity> Get(Expression<Func<UserEntity, bool>> predicate = null)
        {
            return _userRepository.Get(predicate);
        }

        public async Task<UserEntity> GetUserDetails(int userId)
        {
            return await Get(p => p.Id == userId)
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .Include(p=>p.Profile)
                .ThenInclude(p=>p.Reviews)
                .Include(p=>p.Address)
                .Include(p=>p.Bills)
                .Include(p=>p.LibraryItems)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityResult> RegisterUser(UserRegisterRequest userRequest, string role,int? devTeamId)
        {
            var user = new UserEntity
            {
                UserName = userRequest.Username,
                Email = userRequest.Email,
                DevTeamId = devTeamId,
                IsActive = true,
                TermsAccepted = true
            };        
            
            var result = await _userRepository.Register(user, userRequest.Password);

            if (!result.Succeeded)
            {
                
                return result;
            }

            var roleResult = await _userRepository.AddRoleToUser(user, role);

            if (!roleResult.Succeeded)
            {
                return roleResult;
            }

            return result;
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var result = await _userRepository.Login(username, password);

            if (!result.Succeeded)
                return null;

            var dbUser = await _userRepository.GetUserByUsername(username);

            if (dbUser == null)
                return null;

            dbUser.RefreshToken = GenerateRefreshToken();
            dbUser.RefreshTokenExpires = DateTime.Now.AddMinutes(60d);
            await _userRepository.UpdateUser(dbUser);

            return new LoginResponse
            {
                Token = GenerateToken(dbUser),
                RefreshToken = dbUser.RefreshToken,
                User = dbUser,
                Result = result
            };
        }

        public async Task<LoginResponse> RefreshToken(string refreshToken)
        {
            var dbUser = await _userRepository.GetUserByRefreshToken(refreshToken);

            if (dbUser?.RefreshTokenExpires == null ||
                dbUser.RefreshTokenExpires < DateTime.Now)
                return null;

            dbUser.RefreshToken = GenerateRefreshToken();
            dbUser.RefreshTokenExpires = DateTime.Now.AddMinutes(60d);
            await _userRepository.UpdateUser(dbUser);

            return new LoginResponse
            {
                Token = GenerateToken(dbUser),
                RefreshToken = dbUser.RefreshToken,
                User = dbUser
            };
        }

        public async Task<bool?> RevokeRefreshToken(string refreshToken)
        {
            var dbUser = await _userRepository.GetUserByRefreshToken(refreshToken);

            if (dbUser == null)
                return null;

            dbUser.RefreshTokenExpires = DateTime.Now;
            await _userRepository.UpdateUser(dbUser);
            return true;
        }

        private string GenerateToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            claims.AddRange(user.UserRoles.Select(p => p.Role.Name).Select(p => new Claim(ClaimTypes.Role, p)));

            var token = new JwtSecurityToken(
                "https://digitalds.ro",
                "https://digitalds.ro",
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(30d),
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
