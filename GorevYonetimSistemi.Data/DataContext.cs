
using GorevYonetimSistemi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GorevYonetimSistemi.Data
{
    public class DataContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Duty> Duties { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Duty>().ToTable("Duties");
        }
    }
}