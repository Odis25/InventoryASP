using InventoryApp.Services.Models;
using System.Collections.Generic;

namespace InventoryApp.Models.Employee
{
    public class EmployeeIndexModel
    {
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}
