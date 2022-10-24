using Microsoft.EntityFrameworkCore;
using ParserAnimeGO.AnimeData;
using ParserAnimeGO.ConsoleApp.Data.AnimeModels;

namespace ParserAnimeGO.ConsoleApp.Data
{
    internal class ApplicationContext:DbContext
    {
        public DbSet<Anime> Animes { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Dubbing> Dubbing { get; set; } = null!;
        public DbSet<MpaaRate> MpaaRates { get; set; } = null!;
        public DbSet<Studio> Studios { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<TypeAnime> Types { get; set; } = null!;
        public ApplicationContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Memory");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>().HasAlternateKey(u => u.IdFromAnimeGo);
            modelBuilder.Entity<Genre>().HasAlternateKey(u => u.Title);
            modelBuilder.Entity<Studio>().HasAlternateKey(u => u.Title);
            modelBuilder.Entity<Dubbing>().HasAlternateKey(u => u.Title);
            modelBuilder.Entity<MpaaRate>().HasAlternateKey(u => u.Title);
            modelBuilder.Entity<Status>().HasAlternateKey(u => u.Title);
            modelBuilder.Entity<TypeAnime>().HasAlternateKey(u => u.Title);

        }

    }
}
