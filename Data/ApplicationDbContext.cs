using Microsoft.EntityFrameworkCore;
using DrKchida.Models;

namespace DrKchida.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<VideoItem> VideoItems { get; set; }
        public DbSet<Consultation> Consultations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data will be handled in a separate step or here
        }
    }
}
