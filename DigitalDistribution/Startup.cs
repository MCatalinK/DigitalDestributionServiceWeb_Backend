using AutoMapper;
using DigitalDistribution.Helpers;
using DigitalDistribution.HostedServices;
using DigitalDistribution.Middlewares;
using DigitalDistribution.Models.Database;
using DigitalDistribution.Models.Database.Entities;
using DigitalDistribution.Repositories;
using DigitalDistribution.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using System;
using System.Text;

namespace DigitalDistribution
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            #region identity & authorization

            services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;//true
                options.SignIn.RequireConfirmedPhoneNumber = false;

                //options.Password.RequiredLength = 7;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireDigit = true;

                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3d);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddEntityFrameworkStores<DigitalDistributionDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization()
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "https://digitalds.ro",
                        ValidAudience = "https://digitalds.ro",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            @"asdilasjdlnsac213kmopfa-2asda@")),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            #endregion

            services.AddDbContext<DigitalDistributionDbContext>(o => o.UseSqlServer(
               Configuration.GetConnectionString("DigitalServiceDb")));

            //Repositories
                        
            services.AddScoped<BaseRepository<DevelopmentTeamEntity>>();
            services.AddScoped<UpdateRepository>();
            services.AddScoped<BillingAddressRepository>();
            services.AddScoped<ReviewRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<CheckoutItemRepository>();
            services.AddScoped<InvoiceRepository>();
            services.AddScoped<LibraryRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProfileRepository>();

            //Services
            
            services.AddScoped<BaseService<DevelopmentTeamEntity>>();
            services.AddScoped<UpdateService>();
            services.AddScoped<BillingAddressService>();
            services.AddScoped<ReviewService>();
            services.AddScoped<UserService>();
            services.AddScoped<InvoiceService>();
            services.AddScoped<LibraryService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ProfileService>();

            services.AddSingleton(new MapperConfiguration(p => p.AddProfile(new MappingProfile())).CreateMapper());
            services.AddHostedService<SeedDatabaseHostedService>();


            services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


            //services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DigitalDistribution", Version = "v1" });
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithProperty("Application", "DigitalService")
                .Enrich.FromLogContext()
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DigitalDistribution v1"));
            }
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
