using System.Threading.Tasks;

namespace InventoryAppServices.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> LoginAsync(string userName, string password, bool rememberMe);
        Task Logout();
    }
}