using InventoryAppData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryAppData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        Checkout GetByDeviceId(int id);
        Checkout GetByEmployeeId(int id);
        void Add(Checkout newCheckout);
    }
}
