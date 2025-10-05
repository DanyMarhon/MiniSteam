using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniSteam.Entities;

namespace MiniSteam.DataAccess
{
    public class DbDataAccess : IdentityDbContext
    {
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<GenrePerGame> GenrePerGames { get; set; }
        public virtual DbSet<PlatformPerGame> PlatformPerGames { get; set; }

        public DbDataAccess(DbContextOptions<DbDataAccess> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine).EnableDetailedErrors();
    }
}
