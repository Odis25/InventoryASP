using InventoryApp.Data.Common;
using InventoryApp.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ICurrentUserService _currentUserService;

        public AppDbContext(DbContextOptions options, ICurrentUserService currentUserService) 
            : base(options) 
        {
            Database.EnsureCreated();
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "admin".ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "SuperUser", NormalizedName = "superuser".ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "User", NormalizedName = "user".ToUpper() });

            builder.Entity<Position>().HasData(new Position { Id = 1, Name = "Инженер-стажер" });
            builder.Entity<Position>().HasData(new Position { Id = 2, Name = "Инженер" });
            builder.Entity<Position>().HasData(new Position { Id = 3, Name = "Старший инженер" });
            builder.Entity<Position>().HasData(new Position { Id = 4, Name = "Ведущий инженер" });
            builder.Entity<Position>().HasData(new Position { Id = 5, Name = "Зам. начальника отдела" });
            builder.Entity<Position>().HasData(new Position { Id = 6, Name = "Начальник отдела" });

            builder.Entity<Department>().HasData(new Department { Id = 1, Name = "ПНРСУ" });
            builder.Entity<Department>().HasData(new Department { Id = 2, Name = "ТОиС" });

            base.OnModelCreating(builder);
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistories { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entity.State)
                {
                    case EntityState.Modified:
                        entity.Entity.Modified = DateTime.Now;
                        entity.Entity.ModifiedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Added:
                        entity.Entity.Created = DateTime.Now;
                        entity.Entity.CreatedBy = _currentUserService.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
