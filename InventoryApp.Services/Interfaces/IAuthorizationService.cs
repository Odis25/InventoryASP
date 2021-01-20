using System.Threading.Tasks;

namespace InventoryApp.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> LoginAsync(string userName, string password, bool rememberMe);
        Task Logout();
    }
}