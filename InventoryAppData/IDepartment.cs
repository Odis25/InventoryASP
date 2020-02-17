using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IDepartment
    {
        IEnumerable<Department> GetDepartments();
        IEnumerable<Position> GetPositions();
    }
}
