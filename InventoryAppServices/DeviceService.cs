using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAppServices
{
    public class DeviceService : IDevice
    {
        private readonly InventoryContext _context;

        public DeviceService(InventoryContext context)
        {
            _context = context;
        }

        public void Add(Device newDevice)
        {
            _context.Devices.Add(newDevice);
            _context.SaveChanges();
        }

        public void Delete(params int[] idList)
        {
            var devices = _context.Devices.Where(d => idList.Contains(d.Id));

            // Закрываем все чекауты
            var checkouts = _context.Checkouts.Where(c => devices.Contains(c.Device));

            if (checkouts != null)
            {
                _context.RemoveRange(checkouts);
            }

            // Закрываем историю
            var histories = _context.CheckoutHistories.Where(history => devices.Contains(history.Device) && history.CheckedIn == null);

            if (histories != null)
            {
                var now = DateTime.Now;

                foreach (var history in histories)
                {
                    history.CheckedIn = now;
                }
            }
            _context.UpdateRange(devices);

            // Меняем статус устройства на "Deleted"
            foreach (var device in devices)
            {
                device.Status = "Deleted";
            }

            _context.SaveChanges();
        }

        public IEnumerable<Device> GetAll()
        {
            return _context.Devices.Where(d=> d.Status != "Deleted");
        }

        public IEnumerable<Device> GetAvalibleDevices()
        {
            return _context.Devices.Where(device=> device.Status == "Available");
        }

        public Device GetById(int id)
        {
            return _context.Devices.Find(id);
        }

        public Employee GetCurrentHolder(int id)
        {
            var result = _context.Checkouts
                .Include(co => co.Employee)
                .FirstOrDefault(co => co.Device.Id == id)?.Employee;
            return result;
        }

        public IEnumerable<CheckoutHistory> GetDeviceHistory(int id)
        {
            return _context.CheckoutHistories
                .Where(history => history.Device.Id == id);
        }
    }
}
