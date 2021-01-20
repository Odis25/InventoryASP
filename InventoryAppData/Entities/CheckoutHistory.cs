using System;

namespace InventoryApp.Data.Entities
{
    public class CheckoutHistory
    {
        public int Id { get; set; }
        public Device Device { get; set; }
        public Employee Employee { get; set; }

        public DateTime CheckedOut { get; set; }
        public DateTime? CheckedIn { get; set; }
    }
}
