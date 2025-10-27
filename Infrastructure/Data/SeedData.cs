using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            ActiviGoDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            // 1. Categories
            var categories = new[]
            {
                new Category { Name = "Yoga & Mindfulness" },
                new Category { Name = "Cykling" },
                new Category { Name = "Styrketräning" },
                new Category { Name = "Löpning" },
                new Category { Name = "Dans" },
                new Category { Name = "Racketsporter" },
                new Category { Name = "Lagspel" }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // 2. Locations
            var locations = new[]
            {
                new Location { Name = "Centralhallen", Address = "Storgatan 1", IsIndoor = true },
                new Location { Name = "Parken", Address = "Gröna Vägen 2", IsIndoor = false },
                new Location { Name = "Sporthallen", Address = "Idrottsvägen 3", IsIndoor = true }
            };
            context.Locations.AddRange(locations);
            await context.SaveChangesAsync();

            // 3. Activities
            var activities = new[]
            {
                new Activity { Name = "Yoga", Description = "Avslappnande yoga", CategoryId = categories[0].Id, StandardDuration = TimeSpan.FromMinutes(60), Price = 100, ImageUrl = "https://files.nccih.nih.gov/yoga-gettyimages-1204500395-16-9.jpg" },
                new Activity { Name = "Spinning", Description = "Intensiv spinning", CategoryId = categories[1].Id, StandardDuration = TimeSpan.FromMinutes(45), Price = 120, ImageUrl = "https://zycle.eu/imagenes/Indoor-cycle-vs-spinning-cual-es-la-diferencia.jpg" },
                new Activity { Name = "Crossfit", Description = "Styrka och kondition", CategoryId = categories[2].Id, StandardDuration = TimeSpan.FromMinutes(50), Price = 150, ImageUrl = "https://cdn.prod.website-files.com/60b5161b14fe760d9b751c45/65a4cdba170deb8195cb62bb_arnold%20cover%201.webp" },
                new Activity { Name = "Pilates", Description = "Kroppskontroll och styrka", CategoryId = categories[0].Id, StandardDuration = TimeSpan.FromMinutes(60), Price = 110, ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/outdoor-pull-ups-royalty-free-image-1681208481.jpg" },
                new Activity { Name = "Löpning", Description = "Löpning i parken", CategoryId = categories[3].Id, StandardDuration = TimeSpan.FromMinutes(40), Price = 80, ImageUrl = "https://st3.depositphotos.com/1177973/17793/i/450/depositphotos_177932108-stock-photo-group-of-young-people-running.jpg" },
                new Activity { Name = "Zumba", Description = "Dans och träning", CategoryId = categories[4].Id, StandardDuration = TimeSpan.FromMinutes(55), Price = 100, ImageUrl = "https://sas.rochester.edu/dan/assets/images/slider/step_show_063-l.jpg" },
                new Activity { Name = "Badminton", Description = "Racketsport", CategoryId = categories[5].Id, StandardDuration = TimeSpan.FromMinutes(60), Price = 90, ImageUrl = "https://racketnow.com/cdn/shop/collections/CEQxxXK.jpg?v=1727252997&width=2400" },
                new Activity { Name = "Basket", Description = "Lagspel", CategoryId = categories[6].Id, StandardDuration = TimeSpan.FromMinutes(60), Price = 95, ImageUrl = "https://cdn.nba.com/manage/2025/10/BOS_DET_1920x1080.jpg" }
            };
            context.Activities.AddRange(activities);
            await context.SaveChangesAsync();

            // 4. ActivitySessions (20+)
            var sessions = new[]
            {
                new ActivitySession { ActivityId = activities[0].Id, LocationId = locations[0].Id, StartUtc = DateTime.UtcNow.AddDays(1).AddHours(9), EndUtc = DateTime.UtcNow.AddDays(1).AddHours(10), Capacity = 20, IsCanceled = false },
                new ActivitySession { ActivityId = activities[1].Id, LocationId = locations[1].Id, StartUtc = DateTime.UtcNow.AddDays(1).AddHours(11), EndUtc = DateTime.UtcNow.AddDays(1).AddHours(12), Capacity = 15, IsCanceled = false },
                new ActivitySession { ActivityId = activities[2].Id, LocationId = locations[2].Id, StartUtc = DateTime.UtcNow.AddDays(2).AddHours(10), EndUtc = DateTime.UtcNow.AddDays(2).AddHours(11), Capacity = 18, IsCanceled = false },
                new ActivitySession { ActivityId = activities[3].Id, LocationId = locations[0].Id, StartUtc = DateTime.UtcNow.AddDays(2).AddHours(13), EndUtc = DateTime.UtcNow.AddDays(2).AddHours(14), Capacity = 22, IsCanceled = false },
                new ActivitySession { ActivityId = activities[4].Id, LocationId = locations[1].Id, StartUtc = DateTime.UtcNow.AddDays(3).AddHours(9), EndUtc = DateTime.UtcNow.AddDays(3).AddHours(10), Capacity = 16, IsCanceled = false },
                new ActivitySession { ActivityId = activities[5].Id, LocationId = locations[2].Id, StartUtc = DateTime.UtcNow.AddDays(3).AddHours(11), EndUtc = DateTime.UtcNow.AddDays(3).AddHours(12), Capacity = 19, IsCanceled = false },
                new ActivitySession { ActivityId = activities[6].Id, LocationId = locations[0].Id, StartUtc = DateTime.UtcNow.AddDays(4).AddHours(10), EndUtc = DateTime.UtcNow.AddDays(4).AddHours(11), Capacity = 21, IsCanceled = false },
                new ActivitySession { ActivityId = activities[7].Id, LocationId = locations[1].Id, StartUtc = DateTime.UtcNow.AddDays(4).AddHours(12), EndUtc = DateTime.UtcNow.AddDays(4).AddHours(13), Capacity = 17, IsCanceled = false },
                new ActivitySession { ActivityId = activities[0].Id, LocationId = locations[2].Id, StartUtc = DateTime.UtcNow.AddDays(5).AddHours(9), EndUtc = DateTime.UtcNow.AddDays(5).AddHours(10), Capacity = 20, IsCanceled = false },
                new ActivitySession { ActivityId = activities[1].Id, LocationId = locations[0].Id, StartUtc = DateTime.UtcNow.AddDays(5).AddHours(11), EndUtc = DateTime.UtcNow.AddDays(5).AddHours(12), Capacity = 15, IsCanceled = false },
                new ActivitySession { ActivityId = activities[2].Id, LocationId = locations[1].Id, StartUtc = DateTime.UtcNow.AddDays(6).AddHours(10), EndUtc = DateTime.UtcNow.AddDays(6).AddHours(11), Capacity = 18, IsCanceled = false },
                new ActivitySession { ActivityId = activities[3].Id, LocationId = locations[2].Id, StartUtc = DateTime.UtcNow.AddDays(6).AddHours(13), EndUtc = DateTime.UtcNow.AddDays(6).AddHours(14), Capacity = 22, IsCanceled = false },
                new ActivitySession { ActivityId = activities[4].Id, LocationId = locations[0].Id, StartUtc = DateTime.UtcNow.AddDays(7).AddHours(9), EndUtc = DateTime.UtcNow.AddDays(7).AddHours(10), Capacity = 16, IsCanceled = false },
                new ActivitySession { ActivityId = activities[5].Id, LocationId = locations[1].Id, StartUtc = DateTime.UtcNow.AddDays(7).AddHours(11), EndUtc = DateTime.UtcNow.AddDays(7).AddHours(12), Capacity = 19, IsCanceled = false },
                new ActivitySession { ActivityId = activities[6].Id, LocationId = locations[2].Id, StartUtc = DateTime.UtcNow.AddDays(8).AddHours(10), EndUtc = DateTime.UtcNow.AddDays(8).AddHours(11), Capacity = 21, IsCanceled = false },
                new ActivitySession { ActivityId = activities[7].Id, LocationId = locations[0].Id, StartUtc = DateTime.UtcNow.AddDays(8).AddHours(12), EndUtc = DateTime.UtcNow.AddDays(8).AddHours(13), Capacity = 17, IsCanceled = false },
                new ActivitySession { ActivityId = activities[0].Id, LocationId = locations[1].Id, StartUtc = DateTime.UtcNow.AddDays(9).AddHours(9), EndUtc = DateTime.UtcNow.AddDays(9).AddHours(10), Capacity = 20, IsCanceled = false },
                new ActivitySession { ActivityId = activities[1].Id, LocationId = locations[2].Id, StartUtc = DateTime.UtcNow.AddDays(9).AddHours(11), EndUtc = DateTime.UtcNow.AddDays(9).AddHours(12), Capacity = 15, IsCanceled = false },
                new ActivitySession { ActivityId = activities[2].Id, LocationId = locations[0].Id, StartUtc = DateTime.UtcNow.AddDays(10).AddHours(10), EndUtc = DateTime.UtcNow.AddDays(10).AddHours(11), Capacity = 18, IsCanceled = false },
                new ActivitySession { ActivityId = activities[3].Id, LocationId = locations[1].Id, StartUtc = DateTime.UtcNow.AddDays(10).AddHours(13), EndUtc = DateTime.UtcNow.AddDays(10).AddHours(14), Capacity = 22, IsCanceled = false }
            };
            context.ActivitySessions.AddRange(sessions);
            await context.SaveChangesAsync();

            // 4. Users and roles (as in your original code)
            // ... (your user/role creation logic here)
            var user1 = new ApplicationUser { UserName = "user1@test.com", Email = "user1@test.com", EmailConfirmed = true };
            var user2 = new ApplicationUser { UserName = "user2@test.com", Email = "user2@test.com", EmailConfirmed = true };
            var user3 = new ApplicationUser { UserName = "user3@test.com", Email = "user3@test.com", EmailConfirmed = true };
            var admin = new ApplicationUser { UserName = "admin@test.com", Email = "admin@test.com", EmailConfirmed = true };

            await userManager.CreateAsync(user1, "Test123!");
            await userManager.CreateAsync(user2, "Test123!");
            await userManager.CreateAsync(user3, "Test123!");
            await userManager.CreateAsync(admin, "Admin123!");

            await userManager.AddToRoleAsync(user1, "User");
            await userManager.AddToRoleAsync(user2, "User");
            await userManager.AddToRoleAsync(user3, "User");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}