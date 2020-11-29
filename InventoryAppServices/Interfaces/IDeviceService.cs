using InventoryAppServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAppServices.Interfaces
{
    public interface IDeviceService
    {
        Task<ICollection<DeviceDto>> GetDevicesAsync();
        Task<ICollection<DeviceDto>> GetDevicesAsync(bool onlyAvailable);

        Task<DeviceDto> GetDeviceByIdAsync(int id);

        Task CreateDeviceAsync(DeviceDto device);
        Task DeleteDeviceAsync(int id);
        Task UpdateDeviceAsync(DeviceDto device);
    }
}
