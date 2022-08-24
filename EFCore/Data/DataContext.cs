using EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options)
        {

        }
        public DbSet<Product> Users { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products {get; set; }

    }
}
