using DigitalDistribution.Models.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace DigitalDistribution.Repositories
{
    public class UserRepository
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<RoleEntity> _roleManager;

        public UserRepository(UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            SignInManager<UserEntity> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IQueryable<UserEntity> Get(Expression<Func<UserEntity, bool>> predicate = null)
        {
            if (predicate != null)
                return _userManager.Users
                    .Where(predicate);

            return _userManager.Users;
        }

        public async Task<UserEntity> GetUserByUsername(string userName)
        {
            return await _userManager.Users
                .Where(p => p.UserName == userName)
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<UserEntity> GetUserByRefreshToken(string refreshToken)
        {
            return await _userManager.Users
                .Where(p => p.RefreshToken == refreshToken)
                .Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityResult> Register(UserEntity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<SignInResult> Login(string username, string password)
        {
            return await _signInManager.PasswordSignInAsync(username, password, false, true);
        }

        public async Task<IdentityResult> AddRoleToUser(UserEntity user, string role)
        {
            IdentityResult roleResult;
            bool roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                roleResult = await _roleManager.CreateAsync(new RoleEntity()
                {
                    Name = role,
                    NormalizedName = role
                });
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<UserEntity> UpdateUser(UserEntity user)
        {
            await _userManager.UpdateAsync(user);
            return user;
        }
    }
}
