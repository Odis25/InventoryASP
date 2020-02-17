using InventoryAppData.Models;

namespace InventoryASP.Models.Checkouts
{
    public class CheckoutModel
    {
        public Checkout Checkout { get; set; }       
        public bool IsSelected { get; set; }
    }
}
