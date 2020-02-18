using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
            newEmployee.IsActive = true;

            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
        }

        public void Delete(int employeeId)
        {
            var employee = _context.Employees.Find(employeeId);

            // Вернуть все устройства
            var checkouts = _context.Checkouts.Where(c => c.Employee == employee);

            if (checkouts != null)
            {
                _context.RemoveRange(checkouts);
            }

            // Закрыть всю историю использования
            var now = DateTime.Now;

            var histories = _context.CheckoutHistories
                .Where(h => h.Employee == employee && h.CheckedIn == null);

            if (histories != null)
            {
                _context.UpdateRange(histories);
                foreach (var history in histories)
                {                   
                    history.CheckedIn = now;
                }
            }

            // Обновляем статус сотрудника
            _context.Update(employee);
            employee.IsActive = false;

            // Сохраняем изменения
            _context.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees
                .Include(e => e.Checkouts).ThenInclude(c=>c.Device)
                .Include(e => e.Department)
                .Include(e => e.Position);
        }

        public Employee GetById(int employeeId)
        {
            return GetAll()
                .FirstOrDefault(e => e.Id == employeeId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int employeeId)
        {
            return _context.CheckoutHistories
                .Where(ch => ch.Employee.Id == employeeId)
                .Include(ch=>ch.Device);
        }
    }
}
