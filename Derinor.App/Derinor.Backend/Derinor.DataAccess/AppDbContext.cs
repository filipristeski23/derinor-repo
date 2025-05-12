using Derinor.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derinor.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<ProjectBranches> ProjectBranches { get; set; }
        public DbSet<ProjectReports> ProjectReports { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projects>()
                .HasOne(p => p.Users)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProjectBranches>()
                .HasOne(p => p.Projects)
                .WithMany(p => p.ProjectBranches)
                .HasForeignKey(p => p.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProjectReports>()
                .HasOne(p => p.Projects)
                .WithMany(p => p.ProjectReports)
                .HasForeignKey(p => p.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
