using InventoryAppServices.Models;
using System.Collections.Generic;

namespace InventoryASP.Models.Accounts
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
