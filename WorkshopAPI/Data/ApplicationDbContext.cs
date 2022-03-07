using Microsoft.EntityFrameworkCore;
using WorkshopAPI.Models;

namespace WorkshopAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //Mapping Models
        public DbSet<Category> Category { get; set; }
        public DbSet<Movie> Movie{ get; set; }
    }
}
