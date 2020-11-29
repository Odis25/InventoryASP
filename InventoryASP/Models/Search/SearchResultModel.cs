using InventoryAppServices.Models;
using System.Collections.Generic;

namespace InventoryASP.Models.Search
{
    public class SearchResultModel
    {
        public string SearchQuery { get; set; }
        public ICollection<DeviceDto> Devices { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}
