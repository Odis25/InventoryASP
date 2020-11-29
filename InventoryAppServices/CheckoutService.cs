using InventoryAppData;
using InventoryAppData.Entities;
using InventoryAppServices.Common.Helpers;
using InventoryAppServices.Interfaces;
using InventoryAppServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAppServices
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _context;
        private readonly IDepartmentService _departmentService;

        public CheckoutService(AppDbContext context,
            IDepartmentService departmentService)
        {
            _context = context;
            _departmentService = departmentService;
        }

        public async Task CheckInDevice(int deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);

            var checkout = await _context.Checkouts.FirstOrDefaultAsync(c => c.Device.Id == deviceId);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }

            var history = await _context.CheckoutHistories.FirstOrDefaultAsync(h => h.Device.Id == deviceId && h.CheckedIn == null);

            if (history != null)
            {
                history.CheckedIn = DateTime.Now;
            }

            device.Status = DeviceStatus.Available;

            await _context.SaveChangesAsync();
        }
        public async Task CheckOutDevices(int employeeId, params int[] devicesId)
        {
            if (employeeId == 0) return;

            var employee = await _context.Employees.FindAsync(employeeId);

            var now = DateTime.Now;

            foreach (var id in devicesId)
            {
                var deviceEntity = await _context.Devices.FindAsync(id);

                var checkout = new Checkout
                {
                    Employee = employee,
                    Device = deviceEntity,
                    Since = now
                };

                var history = new CheckoutHistory
                {
                    Employee = employee,
                    Device = deviceEntity,
                    CheckedOut = now
                };

                deviceEntity.Status = DeviceStatus.CheckedOut;

                _context.Checkouts.Add(checkout);

                _context.CheckoutHistories.Add(history);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<CheckoutDto> GetByDeviceIdAsync(int id)
        {
            var checkout = await _context.Checkouts
                .Include(c=> c.Employee).ThenInclude(e=> e.Department)
                .Include(c => c.Employee).ThenInclude(e => e.Position)
                .FirstOrDefaultAsync(c => c.Device.Id == id);

            return ConvertToDto(checkout);
        }
        public async Task<ICollection<CheckoutDto>> GetByEmployeeIdAsync(int id)
        {
            var checkouts = await _context.Checkouts
                .Include(c => c.Device)
                .Where(c => c.Employee.Id == id)
                .ToListAsync();

            return ConvertToDto(checkouts);
        }
        public async Task<ICollection<CheckoutHistoryDto>> GetDeviceHistoryByIdAsync(int id)
        {
            var history = await _context.CheckoutHistories
                .Include(h => h.Employee).ThenInclude(e => e.Department)
                .Include(h => h.Employee).ThenInclude(e => e.Position)
                .Where(c => c.Device.Id == id)
                .ToListAsync();

            return ConvertToDto(history);
        }
        public async Task<ICollection<CheckoutHistoryDto>> GetEmployeeHistoryByIdAsync(int id)
        {
            var history = await _context.CheckoutHistories
                .Include(c => c.Device)
                .Where(c => c.Employee.Id == id)
                .ToListAsync();

            return ConvertToDto(history);
        }

        private ICollection<CheckoutHistoryDto> ConvertToDto(IEnumerable<CheckoutHistory> entities)
        {
            return entities.Select(h => new CheckoutHistoryDto
            {
                Id = h.Id,
                Device = new DeviceDto
                {
                    Id = h.Device.Id,
                    DeviceName = h.Device.Name,
                    DeviceType = h.Device.Type,
                    DeviceModel = h.Device.DeviceModel,
                    DeviceManufacturer = h.Device.DeviceModel,
                    SerialNumber = h.Device.SerialNumber,
                    Description = h.Device.Description
                },
                Employee = new EmployeeDto
                {
                    Id = h.Employee.Id,
                    Name = h.Employee.Name,
                    Patronymic = h.Employee.Patronymic,
                    LastName = h.Employee.LastName,
                    ImageUrl = h.Employee.ImageUrl,
                    Department = _departmentService.Departments.FirstOrDefault(d => d.Id == h.Employee.Department.Id),
                    Position = _departmentService.Positions.FirstOrDefault(p => p.Id == h.Employee.Position.Id)
                },
                Since = h.CheckedOut,
                Until = h.CheckedIn
            }).ToHashSet();
        }
        private ICollection<CheckoutDto> ConvertToDto(IEnumerable<Checkout> entities)
        {
            return entities.Select(c => ConvertToDto(c)).ToHashSet();
        }
        private CheckoutDto ConvertToDto(Checkout entity)
        {
            if (entity == null) return null;

            return new CheckoutDto
            {
                Device = new DeviceDto
                {
                    Id = entity.Device.Id,
                    DeviceName = entity.Device.Name,
                    DeviceType = entity.Device.Type,
                    DeviceModel = entity.Device.DeviceModel,
                    DeviceManufacturer = entity.Device.DeviceModel,
                    SerialNumber = entity.Device.SerialNumber,
                    Description = entity.Device.Description
                },
                Employee = new EmployeeDto
                {
                    Id = entity.Employee.Id,
                    Name = entity.Employee.Name,
                    Patronymic = entity.Employee.Patronymic,
                    LastName = entity.Employee.LastName,
                    ImageUrl = entity.Employee.ImageUrl,
                    Department = _departmentService.Departments.FirstOrDefault(d => d.Id == entity.Employee.Department.Id),
                    Position = _departmentService.Positions.FirstOrDefault(p => p.Id == entity.Employee.Position.Id)
                },
                Since = entity.Since
            };
        }
    }
}
