using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IEmployee
    {
        IEnumerable<Employee> GetAll();
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int employeeId);

        Employee GetById(int employeeId);

        void Add(Employee newEmployee);
        void Delete(int employeeId);
        void Update(Employee employee);
    }
}
