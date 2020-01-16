using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IEmployee
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void Add(Employee newEmployee);

        IEnumerable<Device> GetHoldedDevices(int id);
        IEnumerable<CheckoutHistory> GetEmployeeHistory(int id);
    }
}
