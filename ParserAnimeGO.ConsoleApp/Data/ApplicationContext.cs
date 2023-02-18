using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            /*modelBuilder.Entity<Anime>().HasIndex(a => a.IdFromAnimeGo).IsUnique();*/
            /*modelBuilder.Entity<Genre>().HasIndex(g => g.Title).IsUnique();*/
            modelBuilder.Entity<Genre>().HasAlternateKey(u => u.Title);
            /*modelBuilder.Entity<Dubbing>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<MpaaRate>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Status>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Studio>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<TypeAnime>().HasIndex(g => g.Title).IsUnique();*/
        }

        public Task AddRangeAnime(AnimeFromParser animeFromParser)
        {

            return Task.CompletedTask;
        }
    }
}
