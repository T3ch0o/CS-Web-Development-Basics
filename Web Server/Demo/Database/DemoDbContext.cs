namespace Demo.Database
{
    using Demo.Models;

    using Microsoft.EntityFrameworkCore;

    internal class DemoDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Demo;Integrated Security=True");
        }
    }
}