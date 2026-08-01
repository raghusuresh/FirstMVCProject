using FirstMVCProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstMVCProject.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
        {
        

        public DbSet<User> Users { get; set; }
    }
}
