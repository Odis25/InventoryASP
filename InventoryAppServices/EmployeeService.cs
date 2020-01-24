using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAppServices
{
    public class EmployeeService : IEmployee
    {
        private readonly InventoryContext _context;

        public EmployeeService(InventoryContext context)
        {
            _context = context;
        }

        public async Task Add(Employee newEmployee)
        {
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task GiveDevices(IEnumerable<int> idList, int employeeId)
        {
            if (!idList.Any() || employeeId == 0)
                return;

            var employee = _context.Employees.Find(employeeId);
            foreach (var id in idList)
            {
                var device = _context.Devices.Find(id);
                var now = DateTime.Now;
                _context.Checkouts.Add( new Checkout
                {
                    Employee = employee,
                    Device = device,
                    Since = now
                });
            }
            await _context.SaveChangesAsync();

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
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Checkouts)
                    .ThenInclude(c => c.Device)
                .FirstOrDefault(e => e.Id == id);
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
