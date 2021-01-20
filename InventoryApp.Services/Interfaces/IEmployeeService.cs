using InventoryApp.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApp.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ICollection<EmployeeDto>> GetEmployeesAsync();

        Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId);
        
        Task CreateEmployeeAsync(EmployeeDto employee);
        Task DeleteEmployeeAsync(int employeeId);
        Task UpdateEmployeeAsync(EmployeeDto employee);
    }
}
