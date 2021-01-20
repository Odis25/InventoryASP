using System;

namespace InventoryApp.Data.Entities
{
    public class Checkout
    {
        public int Id { get; set; }
        public Device Device { get; set; }
        public Employee Employee { get; set; }
        public DateTime Since { get; set; }
    }
}
