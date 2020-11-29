using InventoryAppServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAppServices.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccountsAsync();
        Task<bool> SaveChanges(IEnumerable<AccountDto> accounts);
    }
}