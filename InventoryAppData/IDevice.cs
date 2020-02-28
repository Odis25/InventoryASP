using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IDevice
    {
        IEnumerable<Device> GetAll();
        IEnumerable<Device> GetAvailableDevices();

        Device GetById(int deviceId);

        void Add(Device newDevice);
        void Delete(int deviceId);
        void Update(Device device);
    }
}
