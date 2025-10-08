using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class ActiviGoDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ActiviGoDbContext(DbContextOptions<ActiviGoDbContext> options) : base(options) { }

        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ActivitySession> ActivitySessions => Set<ActivitySession>();
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Activity -> Category, FK Activity.CategoryId
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Activities)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);  //hinders deletion of connected activities if a category was to be deleted

            // ActivitySession -> Activity, FK ActivitySession.ActivityId
            modelBuilder.Entity<ActivitySession>()
                .HasOne(s => s.Activity)
                .WithMany(a => a.Sessions)
                .HasForeignKey(s => s.ActivityId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade så om Activity tas bort så tas även alla kopplade ActivitySessions bort

            // ActivitySession -> Location, FK ActivitySession.LocationId
            modelBuilder.Entity<ActivitySession>()
                .HasOne(s => s.Location)
                .WithMany(l => l.Sessions)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> ActivitySession, FK Booking.ActivitySessionId
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ActivitySession)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ActivitySessionId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade så om ActitvitySession tas bort så tas även alla kopplade Bookings bort

            // Booking -> ApplicationUser, FK Booking.UserId
            modelBuilder.Entity<Booking>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.Bookings)       // collection på ApplicationUser, finns i Infrastructure
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
