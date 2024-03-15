using Microsoft.EntityFrameworkCore;
using MotorV2Practice.Models;

namespace MotorV2Practice.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>options) : base(options)
        {
        
        }
        public DbSet<Brand> Brands { get; set; }
    }
}
