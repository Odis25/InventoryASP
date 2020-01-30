using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppData
{
    public interface IEmployee
    {
        IEnumerable<Employee> GetAll();
        IEnumerable<Device> GetHoldedDevices(int id);
       
        void Add(Employee newEmployee);
        void Delete(params int[] idList);

        Employee GetById(int id);
    }
}
