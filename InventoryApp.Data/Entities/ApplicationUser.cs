using Microsoft.AspNetCore.Identity;

namespace InventoryApp.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
