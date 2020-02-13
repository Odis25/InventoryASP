using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int deviceId);

        Checkout GetById(int checkoutId);
        Checkout GetLatestCheckout(int deviceId);

        int GetCheckoutHolderId(int deviceId);
        string GetCheckoutHolderFullName(int deviceId);

        void Add(Checkout newCheckout);
        void CheckOutItems(int employeeId, params int[] deviceId);
        void CheckInItem(int deviceId);
    }
}
