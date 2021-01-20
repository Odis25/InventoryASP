using InventoryApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(string username, string password);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
    }
}