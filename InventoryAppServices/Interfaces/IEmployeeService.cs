using InventoryAppServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAppServices.Interfaces
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
