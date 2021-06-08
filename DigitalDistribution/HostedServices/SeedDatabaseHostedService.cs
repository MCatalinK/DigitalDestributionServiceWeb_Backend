using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Models.Database.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalDistribution.HostedServices
{
    public class SeedDatabaseHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private IServiceScopeFactory Services { get; }

        public SeedDatabaseHostedService(IServiceScopeFactory services)
        {
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async _ => await SeedDatabase(),
                null, TimeSpan.Zero, TimeSpan.FromDays(30));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return null;
        }

        public void Dispose()
        {
        }

        private async Task SeedDatabase()
        {
            using var scope = Services.CreateScope();
            await SeedRoles(scope);
        }

        private async Task SeedRoles(IServiceScope scope)
        {
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
            var rolesList = Enum.GetNames<RolesEnum>().ToList();

            foreach (var role in rolesList)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new RoleEntity
                    {
                        Name = role
                    });
                }
            }
        }
    }
}
