﻿using InventoryAppData;
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

        // Получить все устройства
        public IEnumerable<Device> GetAll()
        {
            return _context.Devices.Where(d => d.Status != "Deleted");
        }

        // Получить доступные устройства
        public IEnumerable<Device> GetAvailableDevices()
        {
            return _context.Devices.Where(device => device.Status == "Available");
        }

        // Получить устройство по ID
        public Device GetById(int deviceId)
        {
            return _context.Devices.Find(deviceId);
        }
    }
}
