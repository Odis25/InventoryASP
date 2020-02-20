using InventoryAppData;
using InventoryAppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryAppServices
{
    public class SearchService : ISearch
    {
        private readonly InventoryContext _context;
        
        public SearchService(InventoryContext context)
        {
            _context = context;
        }

        public IEnumerable<Device> SearchDevice(string searchQuery)
        {
            return _context.Devices.Where(device => 
                device.Name.Contains(searchQuery) ||
                device.Type.Contains(searchQuery) ||
                device.Manufacturer.Contains(searchQuery) ||
                device.SerialNumber.Contains(searchQuery) ||
                device.DeviceModel.Contains(searchQuery));
        }

        public IEnumerable<Employee> SearchEmployee(string searchQuery)
        {
            return _context.Employees.Where(employee => 
                employee.Name.Contains(searchQuery) ||
                employee.LastName.Contains(searchQuery) ||
                employee.Patronymic.Contains(searchQuery) ||
                employee.Position.Name.Contains(searchQuery) ||
                employee.Department.Name.Contains(searchQuery))
                .Include(e=>e.Department)
                .Include(e=>e.Position)
                .Include(e=>e.Checkouts);
        }
    }
}
