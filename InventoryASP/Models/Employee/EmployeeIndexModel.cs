using InventoryAppServices.Models;
using System.Collections.Generic;

namespace InventoryASP.Models.Employee
{
    public class EmployeeIndexModel
    {
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}
