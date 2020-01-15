using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAppServices
{
    public class CheckoutService : ICheckout
    {
        private InventoryContext _context;

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
                .Include(c=>c.Employee)
                .FirstOrDefault(c => c.Device.Id == id);
        }

        public Checkout GetByEmployeeId(int id)
        {
            return _context.Checkouts
                .FirstOrDefault(c => c.Employee.Id == id);
        }
    }
}
