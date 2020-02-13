using System.Collections.Generic;

namespace InventoryASP.Models.Device
{
    public class AvailableDevicesModel
    {
        public IEnumerable<DeviceListingModel> Devices { get; set; }
        public int EmployeeId { get; set; }
    }
}
