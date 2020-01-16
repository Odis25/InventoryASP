using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryASP.Data
{
    public class InventoryAppContext : IdentityDbContext
    {
        public InventoryAppContext(DbContextOptions<InventoryAppContext> options)
            : base(options)
        {
        }
    }
}
