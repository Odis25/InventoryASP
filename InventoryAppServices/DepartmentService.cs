using InventoryAppData;
using InventoryAppData.Models;
using System.Collections.Generic;

namespace InventoryAppServices
{
    public class DepartmentService : IDepartment
    {
        private readonly InventoryContext _context;

        public DepartmentService(InventoryContext context)
        {
            _context = context;
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments;
        }
    }
}
