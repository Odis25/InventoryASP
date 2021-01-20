using InventoryApp.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApp.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutDto> GetByDeviceIdAsync(int id);

        Task<ICollection<CheckoutDto>> GetByEmployeeIdAsync(int id);
        Task<ICollection<CheckoutHistoryDto>> GetDeviceHistoryByIdAsync(int id);
        Task<ICollection<CheckoutHistoryDto>> GetEmployeeHistoryByIdAsync(int id);

        Task CheckOutDevices(int employeeId, params int[] devicesId);
        Task CheckInDevice(int deviceId);
    }
}
