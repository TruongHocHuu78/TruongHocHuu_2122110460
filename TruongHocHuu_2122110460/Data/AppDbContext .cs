using TruongHocHuu_2122110460.Model;
using Microsoft.EntityFrameworkCore;


namespace TruongHocHuu_2122110460.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }

}