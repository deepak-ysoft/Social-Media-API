using Microsoft.EntityFrameworkCore;

namespace ZippySocialApi.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Story> Story { get; set; }
    }
}
