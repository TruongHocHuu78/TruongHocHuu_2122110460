using TruongHocHuu_2122110460.Model;
using Microsoft.EntityFrameworkCore;


namespace TruongHocHuu_2122110460.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gallery>()
                .HasOne(g => g.Product)
                .WithMany(p => p.Galleries)
                .HasForeignKey(g => g.ProductId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
    }
    

    
}