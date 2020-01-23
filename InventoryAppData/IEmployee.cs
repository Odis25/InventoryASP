using InventoryAppData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryAppData
{
    public interface IEmployee
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        Task Add(Employee newEmployee);
        Task GiveDevices(IEnumerable<int> idList, int employeeId);


        IEnumerable<Device> GetHoldedDevices(int id);
        IEnumerable<CheckoutHistory> GetEmployeeHistory(int id);
    }
}
