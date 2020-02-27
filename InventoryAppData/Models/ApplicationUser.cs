using Microsoft.AspNetCore.Identity;

namespace InventoryAppData.Models
{
    public class ApplicationUser: IdentityUser
    {
       public string Name { get; set; }
    }
}
