using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        IEnumerable<Checkout> GetByEmployeeId(int id);
        IEnumerable<CheckoutHistory> GetDeviceHistory(int id);
        IEnumerable<CheckoutHistory> GetEmployeeHistory(int id);

        Checkout GetByDeviceId(int id);

        void Add(Checkout newCheckout);       
        void CheckOutDevice(int employeeId, params int[] deviceId);
        void CheckInDevice(IEnumerable<int> deviceId);
    }
}
