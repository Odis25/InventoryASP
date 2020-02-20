using InventoryASP.Models.Device;
using InventoryASP.Models.Employee;
using System.Collections.Generic;

namespace InventoryASP.Models.Search
{
    public class SearchResultModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<DeviceListingModel> Devices { get; set; }
        public IEnumerable<EmployeeListingModel> Employees { get; set; }
    }
}
