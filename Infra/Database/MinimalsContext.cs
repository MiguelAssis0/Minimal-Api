using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalsApi.Domain.Entities;

namespace MinimalsApi.Infra.Database
{
    public class MinimalsContext : DbContext
    {
        public MinimalsContext(DbContextOptions<MinimalsContext> options) : base(options)
        {
            
        }

        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Email = "admin@admin.com",
                    Password = "123456",
                    Profile = "admin"
                }
            );

        }
    }
}