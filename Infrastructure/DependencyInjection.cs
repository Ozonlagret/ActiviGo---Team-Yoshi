using Application.Interfaces;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Options;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Auth;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        // Anropa från Program.cs: services.AddInfrastructure(builder.Configuration)
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ActiviGoDbContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            // Identity (int-nycklar => IdentityRole<int>)
            services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<int>>()                 // RoleManager + rollstöd
            .AddEntityFrameworkStores<ActiviGoDbContext>() // UserStore/RoleStore
            .AddSignInManager()                            // behövs för CheckPasswordSignInAsync
            .AddDefaultTokenProviders();                   // (valfritt) epost/reset tokens

            // JWT options + token generator
            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // Repositories
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivitySessionRepository, ActivitySessionRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IIdentityAuthRepository, IdentityAuthRepository>();

            // Services
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IActivitySessionService, ActivitySessionService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IAuthService, AuthService>();   

            return services;
        }

        // Behåll en parameterlös overload om andra projekt redan kallar den
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
            => throw new InvalidOperationException("Call AddInfrastructure(IConfiguration) instead.");
    }
}