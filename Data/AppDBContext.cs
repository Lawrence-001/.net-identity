using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC.Identity.Models;

namespace MVC.Identity.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> dbContext) : base(dbContext)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
