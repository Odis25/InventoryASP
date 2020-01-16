using InventoryAppData;
using InventoryAppData.Models;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Checkout> GetByEmployeeId(int id)
        {
            return _context.Checkouts
                .Where(c => c.Employee.Id == id);
        }
    }
}
