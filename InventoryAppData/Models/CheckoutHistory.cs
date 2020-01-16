using System;

namespace InventoryAppData.Models
{
    public class CheckoutHistory
    {
        public int Id { get; set; }
        public virtual Device Device { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime CheckedOut { get; set; }
        public DateTime? CheckedIn { get; set; }
    }
}
