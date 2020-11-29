using InventoryAppServices.Common.Enums;

namespace InventoryAppServices.Models
{
    public class AccountDto
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}
