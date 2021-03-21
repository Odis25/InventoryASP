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
    public class DeviceService : IDeviceService
    {
        private readonly AppDbContext _context;
        private readonly ICheckoutService _checkoutService;

        public DeviceService(AppDbContext context, ICheckoutService checkoutService)
        {
            _context = context;
            _checkoutService = checkoutService;
        }


        // Добавить новое устройство
        public async Task CreateDeviceAsync(DeviceDto device)
        {
            var entity = new Device
            {
                Year = device.Year,
                Name = device.DeviceName.Capitalize(),
                Type = device.DeviceType.Capitalize(),
                SerialNumber = device.SerialNumber,
                Manufacturer = device.DeviceManufacturer.Capitalize(),
                DeviceModel = device.DeviceModel.Capitalize(),
                Description = device.Description.Capitalize(),
                Status = DeviceStatus.Available
            };

            _context.Devices.Add(entity);

            await _context.SaveChangesAsync();
        }
        // Удалить устройство
        public async Task DeleteDeviceAsync(int deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);

            if (device == null)
                return;

            // Закрываем чекаут
            var checkout = await _context.Checkouts.FirstOrDefaultAsync(c => c.Device == device);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }

            // Закрываем историю
            var histories = _context.CheckoutHistories.Where(h => h.Device == device && h.CheckedIn == null);

            if (histories != null)
            {
                var now = DateTime.Now;

                foreach (var history in histories)
                {
                    history.CheckedIn = now;
                }
            }

            device.Status = DeviceStatus.Deleted;

            await _context.SaveChangesAsync();
        }
        // Изменить данные устройства
        public async Task UpdateDeviceAsync(DeviceDto device)
        {
            var entity = await _context.Devices.FindAsync(device.Id);

            entity.Year = device.Year;
            entity.Name = device.DeviceName;
            entity.Manufacturer = device.DeviceManufacturer;
            entity.DeviceModel = device.DeviceModel;
            entity.SerialNumber = device.SerialNumber;
            entity.Type = device.DeviceType;
            entity.Description = device.Description;

            await _context.SaveChangesAsync();
        }


        // Получить устройство по ID
        public async Task<DeviceDto> GetDeviceByIdAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            return ConvertToDto(device);
        }


        // Получить все устройства
        public async Task<ICollection<DeviceDto>> GetDevicesAsync()
        {
            return await GetDevicesAsync(string.Empty, false);
        }
        // Получить все доступные устройства
        public async Task<ICollection<DeviceDto>> GetDevicesAsync(bool onlyAvailable)
        {
            return await GetDevicesAsync(string.Empty, onlyAvailable);
        }
        // Получить все устройства соответствующие критерию поиска
        public async Task<ICollection<DeviceDto>> GetDevicesAsync(string searchPattern)
        {
            return await GetDevicesAsync(searchPattern, false);
        }
        // Получить устройства соответствующие критерию поиска и доступности
        public async Task<ICollection<DeviceDto>> GetDevicesAsync(string searchPattern, bool onlyAvailable)
        {
            var devices = _context.Devices.Where(d => onlyAvailable ? d.Status == DeviceStatus.Available : d.Status != DeviceStatus.Deleted);

            if (!string.IsNullOrWhiteSpace(searchPattern))
            {
                devices = devices.Where(d => d.Name.Contains(searchPattern) ||
                            d.Type.Contains(searchPattern) ||
                            d.DeviceModel.Contains(searchPattern) ||
                            d.Manufacturer.Contains(searchPattern) ||
                            d.SerialNumber.Contains(searchPattern) ||
                            d.Year.ToString().Contains(searchPattern));
            }
            devices = devices.OrderBy(d => d.Name);

            return ConvertToDto(await devices.ToListAsync());
        }


        private DeviceDto ConvertToDto(Device entity)
        {
            if (entity == null) return null;

            return new DeviceDto
            {
                Id = entity.Id,
                Year = entity.Year,
                DeviceType = entity.Type,
                DeviceName = entity.Name,
                DeviceModel = entity.DeviceModel,
                DeviceManufacturer = entity.Manufacturer,
                SerialNumber = entity.SerialNumber,
                Created = entity.Created,
                CreatedBy = entity.CreatedBy,
                Modified = entity.Modified,
                ModifiedBy = entity.ModifiedBy,
                Checkout = _checkoutService.GetByDeviceIdAsync(entity.Id).GetAwaiter().GetResult(),
                CheckoutsHistory = _checkoutService.GetDeviceHistoryByIdAsync(entity.Id).GetAwaiter().GetResult()
            };
        }
        private ICollection<DeviceDto> ConvertToDto(IEnumerable<Device> entities)
        {
            return entities.Select(entity => ConvertToDto(entity)).ToHashSet();
        }

    }
}
