using InventoryApp.Data;
using InventoryApp.Services.Common.Helpers;
using InventoryApp.Services.Interfaces;
using InventoryApp.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApp.Services
{
    public class SearchService : ISearchService
    {
        private readonly AppDbContext _context;
        private readonly IDepartmentService _departmentService;

        public SearchService(AppDbContext context, 
            IDepartmentService departmentService)
        {
            _context = context;
            _departmentService = departmentService;
        }

        public async Task<ICollection<DeviceDto>> FindDevicesAsync(string searchQuery)
        {
            var devices = await _context.Devices.Where(device =>
                device.Status != DeviceStatus.Deleted && (
                device.Name.Contains(searchQuery) ||
                device.Type.Contains(searchQuery) ||
                device.Manufacturer.Contains(searchQuery) ||
                device.SerialNumber.Contains(searchQuery) ||
                device.DeviceModel.Contains(searchQuery)))
                .ToListAsync();

            var result = devices.Select(d => new DeviceDto
            {
                Id = d.Id,
                Year = d.Year,
                DeviceName = d.Name,
                DeviceType = d.Type,
                DeviceModel = d.DeviceModel,
                DeviceManufacturer = d.Manufacturer,
                SerialNumber = d.SerialNumber,
                Description = d.Description
            }).ToHashSet();

            return result;
        }

        public async Task<ICollection<EmployeeDto>> FindEmployeesAsync(string searchQuery)
        {
            var employees = await _context.Employees.Where(employee =>
                employee.IsActive && (
                employee.Name.Contains(searchQuery) ||
                employee.LastName.Contains(searchQuery) ||
                employee.Patronymic.Contains(searchQuery) ||
                employee.Position.Name.Contains(searchQuery) ||
                employee.Department.Name.Contains(searchQuery)))
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Checkouts).ThenInclude(c => c.Device).ToListAsync();

            var result = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Patronymic = e.Patronymic,
                LastName = e.LastName,
                ImageUrl = e.ImageUrl,
                Department = _departmentService.Departments.First(d => d.Id == e.Department.Id),
                Position = _departmentService.Positions.First(p => p.Id == e.Position.Id)
            }).ToHashSet();

            return result;
        }
    }
}
