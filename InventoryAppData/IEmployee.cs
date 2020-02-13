using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IEmployee
    {
        IEnumerable<Employee> GetAll();
        IEnumerable<Checkout> GetCheckouts(int employeeId);
        IEnumerable<CheckoutHistory> GetHistory(int employeeId);

        Employee GetById(int employeeId);

        void Add(Employee newEmployee);
        void Delete(int employeeId);
    }
}
