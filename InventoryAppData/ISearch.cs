using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface ISearch
    {
        IEnumerable<Employee> SearchEmployee(string searchQuery);
        IEnumerable<Device> SearchDevice(string searchQuery);
    }
}
