using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task Add(Device newDevice)
        {
            _context.Devices.Add(newDevice);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Device> GetAll()
        {
            return _context.Devices;
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
