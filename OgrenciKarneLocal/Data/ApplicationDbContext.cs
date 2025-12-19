using Microsoft.EntityFrameworkCore;
using OgrenciKarneLocal.Models;

namespace OgrenciKarneLocal.Data
{
    /// <summary>
    /// Entity Framework Core DbContext sınıfı
    /// Veritabanı işlemlerini yönetir
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Students tablosu
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Model yapılandırması
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student tablosu yapılandırması
            modelBuilder.Entity<Student>(entity =>
            {
                // Tablo adı
                entity.ToTable("Students");

                // Primary key
                entity.HasKey(e => e.Id);

                // Ad alanı - zorunlu, maksimum 50 karakter
                entity.Property(e => e.Ad)
                    .IsRequired()
                    .HasMaxLength(50);

                // Soyad alanı - zorunlu, maksimum 50 karakter
                entity.Property(e => e.Soyad)
                    .IsRequired()
                    .HasMaxLength(50);

                // Numara alanı - zorunlu, maksimum 20 karakter
                entity.Property(e => e.Numara)
                    .IsRequired()
                    .HasMaxLength(20);

                // Matematik notu - varsayılan değer 0
                entity.Property(e => e.MatematikNotu)
                    .HasDefaultValue(0);

                // Türkçe notu - varsayılan değer 0
                entity.Property(e => e.TurkceNotu)
                    .HasDefaultValue(0);
            });
        }
    }
}
