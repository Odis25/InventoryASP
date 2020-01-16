using System.Collections.Generic;

namespace InventoryASP.Models.Employee
{
    public class EmployeeListingModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public IEnumerable<InventoryAppData.Models.Device> Devices { get; set; }       
    }
}
