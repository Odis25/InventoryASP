using System.Collections.Generic;

namespace InventoryASP.Models.Employee
{
    public class SelectEmployeeModel
    {
        public int DeviceId { get; set; }

        public List<EmployeeListingModel> Employees { get; set; }
    }
}
