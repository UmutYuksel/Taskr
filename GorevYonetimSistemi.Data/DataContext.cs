using GorevYonetimSistemi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GorevYonetimSistemi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<UserDuty> UserDuties { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDuty>()
                .HasKey(ud => new { ud.UserId, ud.DutyId });

            modelBuilder.Entity<UserDuty>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDuties)
                .HasForeignKey(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserDuty>()
                .HasOne(ud => ud.Duty)
                .WithMany(d => d.UserDuties)
                .HasForeignKey(ud => ud.DutyId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // Özel SaveChanges metodu
        public override int SaveChanges()
        {
            // Silinmiş kullanıcıları tespit et
            var deletedUsers = ChangeTracker.Entries<User>()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity);

            foreach (var user in deletedUsers)
            {
                // Kullanıcıya bağlı görevleri tespit et
                var duties = user.UserDuties.Select(ud => ud.Duty).ToList();

                foreach (var duty in duties)
                {
                    // Eğer görev sadece bu kullanıcıyla ilişkilendirilmişse görevi sil
                    if (duty.UserDuties.Count == 1)
                    {
                        Duties.Remove(duty);
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
