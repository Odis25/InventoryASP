using InventoryApp.Services.Models;
using System.Collections.Generic;

namespace InventoryApp.Models.Accounts
{
    public class IndexModel
    {
        public IndexModel()
        {
            Accounts = new List<AccountDto>();
        }
        public IList<AccountDto> Accounts { get; set; }
    }
}
