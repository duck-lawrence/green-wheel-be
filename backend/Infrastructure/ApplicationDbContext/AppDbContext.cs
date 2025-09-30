using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RentalContract> RentalContracts { get; set; }
    }
}
