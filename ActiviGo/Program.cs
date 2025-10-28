using Application;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Mapping;
using Application.Options;
using Application.Services;
using AutoMapper;
using Domain.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace ActiviGo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Application.Validators.CreateActivitySessionRequestValidator>();

            // Layers, Dependency Injection
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            // CORS
            const string FrontendCors = "FrontendCors";
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy(FrontendCors, p =>
                    p.WithOrigins(
                         "http://localhost:5173",
                         "https://localhost:5173",
                         "http://localhost:5174",
                         "https://localhost:5174"
                     ) // Vite dev (http/https on 5173/5174)
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials());
            });

            // JWT Bearer 
            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ClockSkew = TimeSpan.FromMinutes(1),

                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
                options.AddPolicy("MemberOrAdmin", p => p.RequireRole("Member", "Admin"));
            });

            // Swagger + JWT-knapp
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ActiviGo API", Version = "v1" });
                var scheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter 'Bearer {token}'",
                    Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
                };
                c.AddSecurityDefinition("Bearer", scheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } });
            });

            builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<AutoMapperProfiles>(); });

            var app = builder.Build();

            // Seed data in development
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<ActiviGoDbContext>();
                    var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Domain.Entities.ApplicationUser>>();
                    var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole<int>>>();

                    // Ensure database is created
                    context.Database.EnsureCreated();

                    // Check if already seeded (e.g., if Categories exist)
                    if (!context.Categories.Any())
                    {
                        // Create roles first
                        if (!await roleManager.RoleExistsAsync("Admin"))
                            await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole<int>("Admin"));
                        if (!await roleManager.RoleExistsAsync("User"))
                            await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole<int>("User"));

                        // Seed data
                        await SeedData.InitializeAsync(context, userManager, roleManager);
                    }
                }

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(FrontendCors);
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
