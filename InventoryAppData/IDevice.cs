using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IDevice
    {
        IEnumerable<Device> GetAll();
        Device GetById(int id);
        void Add(Device newDevice);

        IEnumerable<CheckoutHistory> GetDeviceHistory(int id);
        Employee GetCurrentHolder(int id);
    }
}
