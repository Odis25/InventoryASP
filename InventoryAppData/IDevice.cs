using InventoryAppData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAppData
{
    public interface IDevice
    {
        IEnumerable<Device> GetAll();
        IEnumerable<Device> GetAllFreeDevices();
        Device GetById(int id);
        Task Add(Device newDevice);

        IEnumerable<CheckoutHistory> GetDeviceHistory(int id);
        Employee GetCurrentHolder(int id);
    }
}
