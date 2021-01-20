using InventoryApp.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApp.Services.Interfaces
{
    public interface ISearchService
    {
        Task<ICollection<EmployeeDto>> FindEmployeesAsync(string searchQuery);
        Task<ICollection<DeviceDto>> FindDevicesAsync(string searchQuery);
    }
}
