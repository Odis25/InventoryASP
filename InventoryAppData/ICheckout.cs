using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int deviceId);

        Checkout GetById(int checkoutId);
        Checkout GetCheckout(int deviceId);

        void Add(Checkout newCheckout);
        void CheckOutItems(int employeeId, params int[] deviceId);
        void CheckInItem(int deviceId);
    }
}
