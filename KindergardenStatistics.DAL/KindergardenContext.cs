using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace KindergardenStatistics.DAL
{
    public class KindergardenContext : DbContext
    {
        public KindergardenContext(DbContextOptions<KindergardenContext> options) : base(options)
        {
        }

        public DbSet<Kindergarden> Kindergarden { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Child> Child { get; set; }
        public DbSet<GroupChild> GroupChild { get; set; }
        public DbSet<Attendance> Attendance { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kindergarden>().ToTable("Kindergarden");
            modelBuilder.Entity<Group>().ToTable("Group");
            modelBuilder.Entity<Child>().ToTable("Child");
            modelBuilder.Entity<GroupChild>().ToTable("GroupChild");
            modelBuilder.Entity<Attendance>().ToTable("Attendance");
        }
    }
}
