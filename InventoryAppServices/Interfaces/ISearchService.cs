using InventoryAppServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAppServices.Interfaces
{
    public interface ISearchService
    {
        Task<ICollection<EmployeeDto>> FindEmployeesAsync(string searchQuery);
        Task<ICollection<DeviceDto>> FindDevicesAsync(string searchQuery);
    }
}
