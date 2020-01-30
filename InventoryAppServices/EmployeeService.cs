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
            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
        }

        public void Delete(params int[] idList)
        {
            var employees = _context.Employees.Where(e => idList.Contains(e.Id));

            // Вернуть все устройства
            var checkouts = _context.Checkouts.Where(c => employees.Contains(c.Employee));
            if (checkouts != null)
            {
                _context.RemoveRange(checkouts);
            }

            // Закрыть всю историю использования
            var now = DateTime.Now;
            var histories = _context.CheckoutHistories
                .Where(h => employees.Contains(h.Employee) && h.CheckedIn == null);
            if (histories != null)
            {
                foreach (var history in histories)
                {
                    _context.Update(history);
                    history.CheckedIn = now;
                }
            }
            _context.SaveChanges();
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

        public IEnumerable<Device> GetHoldedDevices(int id)
        {
            return _context.Employees
                .Find(id).Checkouts.Select(c => c.Device);
        }


    }
}
