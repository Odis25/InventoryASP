using InventoryApp.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApp.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<ICollection<DeviceDto>> GetDevicesAsync();
        Task<ICollection<DeviceDto>> GetDevicesAsync(bool onlyAvailable);
        Task<ICollection<DeviceDto>> GetDevicesAsync(string searchPattern);
        Task<ICollection<DeviceDto>> GetDevicesAsync(string searchPattern, bool onlyAvailable);

        Task<DeviceDto> GetDeviceByIdAsync(int id);

        Task CreateDeviceAsync(DeviceDto device);
        Task DeleteDeviceAsync(int id);
        Task UpdateDeviceAsync(DeviceDto device);
    }
}
