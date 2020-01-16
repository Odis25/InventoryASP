using System;

namespace InventoryAppData.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public virtual Device Device { get; set; }
        public virtual Employee Employee { get; set; }

        public DateTime Since { get; set; }
    }
}
