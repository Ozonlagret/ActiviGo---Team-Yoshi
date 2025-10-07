using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ActiviGoDbContext : DbContext
    {
        public ActiviGoDbContext(DbContextOptions<ActiviGoDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ActivitySession> ActivitySessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Activities)
                .OnDelete(DeleteBehavior.Restrict); //hinders deletion of connected activities if a category was to be deleted

            modelBuilder.Entity<ActivitySession>()
                .HasOne(s => s.Activity)
                .WithMany(a => a.Sessions)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivitySession>()
                .HasOne(s => s.Location)
                .WithMany(l => l.Sessions)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ActivitySession)
                .WithMany(s => s.Bookings)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
