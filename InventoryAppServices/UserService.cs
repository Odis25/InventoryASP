using InventoryAppData.Entities;
using InventoryAppServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;

namespace InventoryAppServices
{
    public class UserService : IUserService
    {
        private const string DisplayNameAttribute = "DisplayName";
        private const string GivenNameAttribute = "GivenName";
        private const string SnAttribute = "Sn";
        private const string SAMAccountNameAttribute = "SAMAccountName";

        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<ApplicationUser> GetUserAsync(string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, "INCOMSYSTEM.ru"))
            {
                // Проверка логина и пароля
                if (context.ValidateCredentials(username, password))
                {
                    var user = await _userManager.FindByNameAsync(username);

                    if (user == null)
                    {
                        // Поиск пользователя в ActiveDirectory
                        using (var entry = new DirectoryEntry("LDAP://incomsystem.ru", "INCOMSYSTEM\\" + username, password))
                        {
                            using (var searcher = new DirectorySearcher(entry))
                            {
                                // Критерий поиска
                                searcher.Filter = $"SAMAccountName={username}";

                                // Получаемые свойства
                                searcher.PropertiesToLoad.Add(SAMAccountNameAttribute);
                                searcher.PropertiesToLoad.Add(DisplayNameAttribute);
                                searcher.PropertiesToLoad.Add(GivenNameAttribute);
                                searcher.PropertiesToLoad.Add(SnAttribute);

                                var result = searcher.FindOne();

                                if (result != null)
                                {
                                    var accountName = result.Properties[SAMAccountNameAttribute][0].ToString();
                                    var displayName = result.Properties[DisplayNameAttribute][0].ToString();

                                    var nameArray = displayName.Split(' ');

                                    var name = nameArray[1];
                                    var lastName = nameArray[0];

                                    user = new ApplicationUser
                                    {
                                        UserName = accountName,
                                        Name = $"{lastName} {name}"
                                    };

                                    await _userManager.CreateAsync(user);

                                    if (user.UserName.Equals("budanovav", StringComparison.OrdinalIgnoreCase) ||
                                        user.UserName.Equals("belovvv", StringComparison.OrdinalIgnoreCase) ||
                                        user.UserName.Equals("nigmatovrr", StringComparison.OrdinalIgnoreCase))
                                    {
                                        await _userManager.AddToRoleAsync(user, "Admin");
                                    }
                                    else
                                    {
                                        await _userManager.AddToRoleAsync(user, "User");
                                    }
                                }
                            }
                        }
                    }
                    return user;
                }
                return null;
            }
        }
    }
}
