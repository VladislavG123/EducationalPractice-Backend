using GreenPoint.Dotnet.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace GreenPoint.Dotnet.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            try
            {
                // It should throw exception when migrations are not available,
                // for example in a tests
                Database.Migrate();
            }
            catch (InvalidOperationException)
            {
                Database.EnsureCreated();
            }

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<SpotImage> SpotImages { get; set; }
        public DbSet<UserCode> UserCodes { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var admin = new Admin
            {
                CreationDate = DateTime.Now,
                Login = "dev",
                Password = BCrypt.Net.BCrypt.HashPassword("123123")
            };
            modelBuilder.Entity<Admin>().HasData(admin);
        }

    }
}