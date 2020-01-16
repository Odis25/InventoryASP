using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAppServices
{
    public class EmployeeService : IEmployee
    {
        private readonly InventoryContext _context;

        public EmployeeService(InventoryContext context)
        {
            _context = context;
        }
        public void Add(Employee newEmployee)
        {
            _context.Employees.Add(newEmployee);
            _context.SaveChangesAsync();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Checkouts)
                    .ThenInclude(c => c.Device);
        }

        public Employee GetById(int id)
        {
            return _context.Employees.Find(id);
        }

        public IEnumerable<CheckoutHistory> GetEmployeeHistory(int id)
        {
            return _context.CheckoutHistories
                .Where(co => co.Employee.Id == id);
        }

        public IEnumerable<Device> GetHoldedDevices(int id)
        {
            return _context.Employees
                .Find(id).Checkouts.Select(c => c.Device);
        }
    }
}
