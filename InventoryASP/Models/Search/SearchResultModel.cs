using InventoryApp.Services.Models;
using System.Collections.Generic;

namespace InventoryApp.Models.Search
{
    public class SearchResultModel
    {
        public string SearchQuery { get; set; }
        public ICollection<DeviceDto> Devices { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}
