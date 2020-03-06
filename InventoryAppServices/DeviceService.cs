using InventoryAppData;
using InventoryAppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAppServices
{
    public class DeviceService : IDevice
    {
        private readonly InventoryContext _context;

        public DeviceService(InventoryContext context)
        {
            _context = context;
        }

        // Добавить новое устройство
        public void Add(Device newDevice)
        {
            newDevice.Status = "Available";

            _context.Devices.Add(newDevice);
            _context.SaveChanges();
        }

        // Удалить устройство
        public void Delete(int deviceId)
        {
            var device = _context.Devices.Find(deviceId);

            if (device == null)
                return;

            // Закрываем чекаут
            var checkout = _context.Checkouts
                .FirstOrDefault(checkout => checkout.Device == device);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }

            // Закрываем историю
            var histories = _context.CheckoutHistories
                .Where(history => history.Device == device && history.CheckedIn == null);

            if (histories != null)
            {
                var now = DateTime.Now;
                foreach (var history in histories)
                {
                    history.CheckedIn = now;
                }
            }

            // Меняем статус устройства на "Deleted"
            _context.Update(device);
            device.Status = "Deleted";

            _context.SaveChanges();
        }

        // Изменить данные устройства
        public void Update(Device device)
        {
            var modifiedDevice = _context.Devices.Find(device.Id);

            modifiedDevice.Name = device.Name;
            modifiedDevice.Manufacturer = device.Manufacturer;
            modifiedDevice.DeviceModel = device.DeviceModel;
            modifiedDevice.SerialNumber = device.SerialNumber;
            modifiedDevice.Type = device.Type;
            modifiedDevice.Description = device.Description;

            _context.SaveChanges();
        }

        // Получить все устройства
        public IEnumerable<Device> GetAll()
        {
            return _context.Devices
                .Where(d => d.Status != "Deleted")
                .OrderBy(d=>d.Name);
        }

        // Получить доступные устройства
        public IEnumerable<Device> GetAvailableDevices()
        {
            return _context.Devices
                .Where(d => d.Status == "Available")
                .OrderBy(d=>d.Name);
        }

        // Получить устройство по ID
        public Device GetById(int deviceId)
        {
            return _context.Devices.Find(deviceId);
        }
    }
}
