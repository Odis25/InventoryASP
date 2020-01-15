using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAppServices
{
    public class DeviceService : IDevice
    {
        private InventoryContext _context;

        public DeviceService(InventoryContext context)
        {
            _context = context;
        }

        public void Add(Device newDevice)
        {
            _context.Devices.Add(newDevice);
            _context.SaveChanges();
        }

        public IEnumerable<Device> GetAll()
        {
            return _context.Devices;
        }

        public Device GetById(int id)
        {
            return _context.Devices
                .FirstOrDefault(device => device.Id == id);
        }

        public Employee GetCurrentHolder(int id)
        {
            var result = _context.Checkouts
                .Include(co => co.Employee)
                .FirstOrDefault(co => co.Device.Id == id).Employee;
            return result;
        }

        public IEnumerable<CheckoutHistory> GetDeviceHistory(int id)
        {
            return _context.CheckoutHistories
                .Where(history => history.Device.Id == id);
        }
    }
}
