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

        // Получить все устройства
        public async Task<ICollection<DeviceDto>> GetDevicesAsync()
        {
            return await GetDevicesAsync(false);
        }

        // Получить доступные устройства
        public async Task<ICollection<DeviceDto>> GetDevicesAsync(bool onlyAvailable)
        {
            var devices = await _context.Devices
                .Where(d => onlyAvailable ? d.Status == DeviceStatus.Available : d.Status != DeviceStatus.Deleted)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return ConvertToDto(devices);
        }

        // Получить устройство по ID
        public async Task<DeviceDto> GetDeviceByIdAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            return ConvertToDto(device);
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
