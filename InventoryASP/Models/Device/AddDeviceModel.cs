using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryASP.Models.Device
{
    public class AddDeviceModel
    {
        public IEnumerable<DeviceListingModel> Devices { get; set; }
        public int EmployeeId { get; set; }
    }
}
