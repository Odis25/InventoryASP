using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IDevice
    {
        IEnumerable<Device> GetAll();
        IEnumerable<Device> GetAvalibleDevices();
        IEnumerable<CheckoutHistory> GetDeviceHistory(int id);

        void Add(Device newDevice);
        void Delete(params int[] idList);

        Device GetById(int id);
        Employee GetCurrentHolder(int id);
    }
}
