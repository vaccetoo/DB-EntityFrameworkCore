using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        private const string ConnectionString = "Server=Asus-Tuf;Database=StudentSystem;Integrated Security=True;";
        protected FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Town> Towns { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
