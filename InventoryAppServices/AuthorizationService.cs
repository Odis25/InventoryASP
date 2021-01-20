using InventoryApp.Data.Entities;
using InventoryApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace InventoryApp.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthorizationService(IUserService userService, SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(string userName, string password, bool rememberMe)
        {
            var user = await _userService.GetUserAsync(userName, password);

            if (user != null)
            {
                await _signInManager.SignInAsync(user, rememberMe);

                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
