using InventoryApp.Data;
using InventoryApp.Data.Entities;
using InventoryApp.Services.Common.Extensions;
using InventoryApp.Services.Common.Helpers;
using InventoryApp.Services.Interfaces;
using InventoryApp.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly ICheckoutService _checkoutService;
        private readonly IDepartmentService _departmentService;

        public EmployeeService(AppDbContext context,
            ICheckoutService checkoutService,
            IDepartmentService departmentService)
        {
            _context = context;
            _checkoutService = checkoutService;
            _departmentService = departmentService;
        }

        // Добавить нового сотрудника
        public async Task CreateEmployeeAsync(EmployeeDto employee)
        {
            var position = await _context.Positions.FindAsync(employee.Position.Id);

            var department = await _context.Departments.FindAsync(employee.Department.Id);

            var entity = new Employee
            {
                Name = employee.Name.Capitalize(),
                LastName = employee.LastName.Capitalize(),
                Patronymic = employee.Patronymic.Capitalize(),
                ImageUrl = employee.ImageUrl,
                Position = position,
                Department = department,
                IsActive = true
            };

            _context.Employees.Add(entity);

            await _context.SaveChangesAsync();
        }

        // Удалить сотрудника
        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.Checkouts).ThenInclude(c => c.Device)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            var checkouts = _context.Checkouts.Where(c => c.Employee == employee);

            if (checkouts != null)
            {
                foreach (var checkout in checkouts)
                {
                    checkout.Device.Status = DeviceStatus.Available;
                }
                _context.RemoveRange(checkouts);
            }

            var histories = _context.CheckoutHistories.Where(h => h.Employee == employee && h.CheckedIn == null);

            if (histories != null)
            {
                var now = DateTime.Now;

                foreach (var history in histories)
                {
                    history.CheckedIn = now;
                }
            }

            employee.IsActive = false;

            await _context.SaveChangesAsync();
        }

        // Получить список всех сотрудников
        public async Task<ICollection<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.LastName)
                .Include(e => e.Department)
                .Include(e => e.Position)
                .ToListAsync();

            return ConvertToDto(employees);
        }

        // Получить сотрудника по Id
        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Position)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            return ConvertToDto(employee);
        }

        // Изменить данные сотрудника
        public async Task UpdateEmployeeAsync(EmployeeDto employee)
        {
            var entity = await _context.Employees.FindAsync(employee.Id);
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == employee.Position.Id);
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == employee.Department.Id);

            entity.ImageUrl = employee.ImageUrl;
            entity.LastName = employee.LastName.Capitalize();
            entity.Name = employee.Name.Capitalize();
            entity.Patronymic = employee.Patronymic.Capitalize();
            entity.Position = position;
            entity.Department = department;

            await _context.SaveChangesAsync();
        }

        private EmployeeDto ConvertToDto(Employee entity)
        {
            if (entity == null) return null;

            return new EmployeeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                LastName = entity.LastName,
                Patronymic = entity.Patronymic,
                ImageUrl = entity.ImageUrl,
                Department = _departmentService.Departments.First(d => d.Id == entity.Department.Id),
                Position = _departmentService.Positions.First(p => p.Id == entity.Position.Id),
                Checkouts = _checkoutService.GetByEmployeeIdAsync(entity.Id).GetAwaiter().GetResult(),
                CheckoutHistory = _checkoutService.GetEmployeeHistoryByIdAsync(entity.Id).GetAwaiter().GetResult()
            };
        }
        private ICollection<EmployeeDto> ConvertToDto(IEnumerable<Employee> entities)
        {
            return entities.Select(entity => ConvertToDto(entity)).ToHashSet();
        }
    }
}
