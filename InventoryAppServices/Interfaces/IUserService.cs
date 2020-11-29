using InventoryAppData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAppServices.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(string username, string password);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
    }
}