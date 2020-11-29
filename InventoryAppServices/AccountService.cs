using InventoryAppData.Entities;
using InventoryAppServices.Common.Enums;
using InventoryAppServices.Interfaces;
using InventoryAppServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryAppServices
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsAsync()
        {
            var users = await _userService.GetUsersAsync();
            var result = new HashSet<AccountDto>();

            foreach (var user in users)
            {
                var role = Enum.Parse<Role>((await _userManager.GetRolesAsync(user)).First());
                var account = new AccountDto
                {
                    Id = user.Id,
                    AccountName = user.UserName,
                    UserName = user.Name,
                    Role = role
                };
                result.Add(account);
            }

            return result;
        }

        public async Task<bool> SaveChanges(IEnumerable<AccountDto> accounts)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var account in accounts)
            {
                var user = await _userManager.FindByIdAsync(account.Id);
                var roles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.AddToRoleAsync(user, account.Role.ToString());

                if (user.Id == userId)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, false);

                    if (account.Role != Role.Admin)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
