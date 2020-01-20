using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IDepartment
    {
        IEnumerable<Department> GetAll();
    }
}
