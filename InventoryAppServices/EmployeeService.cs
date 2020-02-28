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

        // Добавить нового сотрудника
        public void Add(Employee newEmployee)
        {
            var position = _context.Positions.Find(newEmployee.Position.Id);
            var department = _context.Departments.Find(newEmployee.Department.Id);

            newEmployee.IsActive = true;
            newEmployee.Position = position;
            newEmployee.Department = department;

            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
        }

        // Удалить сотрудника
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

        // Получить список всех сотрудников
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.Where(e => e.IsActive).OrderBy(e=> e.LastName)
                .Include(e => e.Checkouts).ThenInclude(c => c.Device)
                .Include(e => e.Department)
                .Include(e => e.Position);
        }

        // Получить сотрудника по Id
        public Employee GetById(int employeeId)
        {
            return GetAll()
                .FirstOrDefault(e => e.Id == employeeId);
        }

        // Получить историю использования оборудования сотрудником
        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int employeeId)
        {
            return _context.CheckoutHistories
                .Where(ch => ch.Employee.Id == employeeId)
                .OrderByDescending(ch=>ch.CheckedIn)
                .Include(ch => ch.Device);
        }

        // Изменить данные сотрудника
        public void Update(Employee employee)
        {
            var modifiedEmployee = _context.Employees.Find(employee.Id);
            
            var position = _context.Positions.Find(employee.Position.Id);
            var department = _context.Departments.Find(employee.Department.Id);

            modifiedEmployee.ImageUrl = employee.ImageUrl;
            modifiedEmployee.LastName = employee.LastName;
            modifiedEmployee.Name = employee.Name;
            modifiedEmployee.Patronymic = employee.Patronymic;
            modifiedEmployee.Position = position;
            modifiedEmployee.Department = department;

            _context.SaveChanges();
        }
    }
}
