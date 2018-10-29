namespace Torshia.Database
{
    using Microsoft.EntityFrameworkCore;

    using Torshia.Models;

    internal class TorshiaDbContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Torshia;Integrated Security=True")
                          .UseLazyLoadingProxies();
        }
    }
}