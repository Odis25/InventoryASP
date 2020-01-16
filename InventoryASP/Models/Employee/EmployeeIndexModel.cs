using System.Collections.Generic;

namespace InventoryASP.Models.Employee
{
    public class EmployeeIndexModel
    {
        public IEnumerable<EmployeeListingModel> Employees { get; set; }
    }
}
