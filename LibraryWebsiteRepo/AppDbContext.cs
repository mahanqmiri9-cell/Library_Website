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
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);


                entity.Property(u => u.FullName)
                      .HasMaxLength(100);

                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.PhoneNumber)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(u => u.Role)
                      .IsRequired();

                entity.Property(u => u.CreatedAt)
                      .IsRequired();

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.PhoneNumber).IsUnique();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Title)
                      .IsRequired()   
                      .HasMaxLength(100);

                entity.Property(u => u.ISBN)
                      .IsRequired();

                entity.Property(u => u.Categoryid)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(u => u.Aythorid)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(u => u.Dercription)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.PublishYear)
                      .IsRequired()
                      .HasMaxLength(4);

                entity.Property(u => u.TotalCopies)
                      .IsRequired();

                entity.Property(u => u.AvaillableCopies)
                      .IsRequired();

                entity.Property(u => u.CreatedAt)
                      .IsRequired();

                entity.HasIndex(u => u.ISBN).IsUnique();



            });
        }

    }
}