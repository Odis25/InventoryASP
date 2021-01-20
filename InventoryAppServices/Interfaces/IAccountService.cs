using InventoryApp.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApp.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccountsAsync();
        Task<bool> SaveChanges(IEnumerable<AccountDto> accounts);
    }
}