using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();        
        Checkout GetByDeviceId(int id);
        void Add(Checkout newCheckout);
    }
}
