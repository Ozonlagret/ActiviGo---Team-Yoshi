using Application.Interfaces;
using Application.Options;
using Domain.Entities;
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
        // Anropa denna fr�n Program.cs: services.AddInfrastructure(builder.Configuration)
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
                // o.Password.RequiredLength = 6; osv (valfritt)
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ActiviGoDbContext>();

            // JWT options + token generator
            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }

        // Beh�ll en parameterl�s overload om andra projekt redan kallar den
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
            => throw new InvalidOperationException("Call AddInfrastructure(IConfiguration) instead.");
    }
}