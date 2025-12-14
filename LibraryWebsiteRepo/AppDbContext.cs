using LibraryWebsite.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebsite.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);


                entity.Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                      .HasMaxLength(500);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(u => u.PhoneNumber)
                      .HasMaxLength(20);

                entity.Property(u => u.Role)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(u => u.CreatedAt)
                      .IsRequired();

                entity.Property(u => u.UpdatedAt);

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }

    }
}
