using InventoryAppData;
using InventoryAppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InventoryAppServices
{
    public class CheckoutService : ICheckout
    {
        private readonly InventoryContext _context;

        public CheckoutService(InventoryContext context)
        {
            _context = context;
        }

        public void Add(Checkout newCheckout)
        {
            _context.Checkouts.Add(newCheckout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetByDeviceId(int id)
        {
            return _context.Checkouts
                .FirstOrDefault(c => c.Device.Id == id);
        }

        public IEnumerable<CheckoutHistory> GetDeviceHistory(int id)
        {
            return _context.CheckoutHistories
                .Where(h => h.Device.Id == id)
                .Include(h => h.Employee);
        }

        public IEnumerable<CheckoutHistory> GetEmployeeHistory(int id)
        {
            return _context.CheckoutHistories
                .Where(h => h.Employee.Id == id)
                .Include(h => h.Device);
        }

        public void CheckOutDevice(int employeeId, params int[] deviceId)
        {
            if (employeeId == 0 || deviceId.Length == 0)
                return;

            var employee = _context.Employees.Find(employeeId);
            var now = DateTime.Now;


            foreach (var id in deviceId)
            {
                // создаем запись о получении оборудования
                var device = _context.Devices.Find(id);
                _context.Checkouts.Add(new Checkout
                {
                    Employee = employee,
                    Device = device,
                    Since = now
                });
                device.Status = "Checked Out";

                // Создаем историю использования
                _context.CheckoutHistories.Add(new CheckoutHistory
                {
                    Employee = employee,
                    Device = device,
                    CheckedOut = now
                });
            }

            _context.SaveChanges();
        }

        public void CheckInDevice(IEnumerable<int> deviceId)
        {
            var now = DateTime.Now;

            foreach (var id in deviceId)
            {
                var device = _context.Devices.Find(id);
                var checkout = _context.Checkouts.FirstOrDefault(c => c.Device == device);
                var history = _context.CheckoutHistories.FirstOrDefault(h => h.Device == device && h.CheckedIn == null);

                _context.Update(device);
                device.Status = "Available";

                _context.Update(history);
                history.CheckedIn = now;

                _context.Remove(checkout);
            }

            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetByEmployeeId(int id)
        {
            return _context.Checkouts.Where(c => c.Employee.Id == id);
        }
    }
}
