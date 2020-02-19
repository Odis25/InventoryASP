using InventoryAppData.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryAppData
{
    public class InventoryContext : IdentityDbContext<ApplicationUser>
    {
        public InventoryContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistories { get; set; }
    }
}
