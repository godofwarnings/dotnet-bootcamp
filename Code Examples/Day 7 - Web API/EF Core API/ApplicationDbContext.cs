using Microsoft.EntityFrameworkCore;
using ProductApiEF.Models;

namespace ProductApiEF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
